using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services;
using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardiansExpress.Controllers
{
    public class BalanceSheetController : Controller
    {
        private readonly IBalanceSheetService _reportService;
        private readonly MyDbContext _context;

        public BalanceSheetController(IBalanceSheetService reportService, MyDbContext context)
        {
            _reportService = reportService;
            _context = context;
        }

        public IActionResult BalanceSheetIndex()
        {
            // Get unique account groups for dropdown/datalist
            ViewBag.UniqueAccountGroups = _context.ledgerEntity
                .Where(a => a.IsDeleted == false)
                .Select(a => a.AccGroup)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            ViewBag.ACCGroup = _context.ledgerEntity
                .Where(a => a.IsDeleted == false)
                .Select(a => a.AccHead)
                .Distinct()
                .ToList();

            // Return empty list initially - data will load only after search
            return View(new List<LedgerMasterEntity>());
        }

        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate, string branch, string accGroup)
        {
            // Populate ViewBag data for the view (needed for dropdowns)
            ViewBag.UniqueAccountGroups = _context.ledgerEntity
                .Where(a => a.IsDeleted == false)
                .Select(a => a.AccGroup)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            ViewBag.ACCGroup = _context.ledgerEntity
                .Where(a => a.IsDeleted == false)
                .Select(a => a.AccHead)
                .Distinct()
                .ToList();

            // Validate at least one filter is provided
            if (string.IsNullOrEmpty(accGroup) &&
                !startDate.HasValue &&
                !endDate.HasValue &&
                string.IsNullOrEmpty(branch))
            {
                ModelState.AddModelError("", "Please provide at least one search criteria");
                return View("BalanceSheetIndex", new List<LedgerMasterEntity>());
            }

            // Pass selected values back to view for persistence
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedBranch = branch;
            ViewBag.SelectedAccGroup = accGroup;

            // Get report data with exact AccGroup match
            var reportData = _reportService.GetAll(startDate, endDate, branch, accGroup);

            // Filter by exact AccGroup if specified
            if (!string.IsNullOrWhiteSpace(accGroup))
            {
                reportData = reportData.Where(r =>
                    !string.IsNullOrEmpty(r.AccGroup) &&
                    r.AccGroup.Equals(accGroup, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Group the data to eliminate duplicates and sum amounts
            var groupedData = GroupLedgerData(reportData.ToList());

            return View("BalanceSheetIndex", groupedData);
        }

        private List<LedgerMasterEntity> GroupLedgerData(List<LedgerMasterEntity> data)
        {
            if (data == null || !data.Any())
                return new List<LedgerMasterEntity>();

            var groupedData = data
                .Where(x => !string.IsNullOrEmpty(x.AccGroup))
                .GroupBy(x => x.AccGroup.Trim().ToUpper())
                .Select(g =>
                {
                    var firstItem = g.First();
                    decimal totalDebitAmount = 0;
                    decimal totalCreditAmount = 0;
                    string balanceType = "CR"; // Default to Credit

                    foreach (var item in g)
                    {
                        decimal amount = item.BalanceOpening ?? 0;

                        if (item.BalanceIn?.ToUpper() == "DEBIT" || item.BalanceIn?.ToUpper() == "DR")
                        {
                            totalDebitAmount += amount;
                        }
                        else if (item.BalanceIn?.ToUpper() == "CREDIT" || item.BalanceIn?.ToUpper() == "CR")
                        {
                            totalCreditAmount += amount;
                        }
                    }

                    // Determine the net balance and type
                    decimal netBalance = 0;
                    if (totalDebitAmount > totalCreditAmount)
                    {
                        netBalance = totalDebitAmount - totalCreditAmount;
                        balanceType = "DR";
                    }
                    else if (totalCreditAmount > totalDebitAmount)
                    {
                        netBalance = totalCreditAmount - totalDebitAmount;
                        balanceType = "CR";
                    }
                    else
                    {
                        netBalance = 0;
                        balanceType = "NIL";
                    }

                    return new LedgerMasterEntity
                    {
                        AccGroup = firstItem.AccGroup,
                        BalanceOpening = netBalance,
                        BalanceIn = balanceType,
                        // Copy other properties if needed
                        AccHead = firstItem.AccHead,
                        LedgerId = firstItem.LedgerId
                    };
                })
                .OrderBy(x => x.AccGroup)
                .ToList();

            return groupedData;
        }

        [HttpGet]
        public IActionResult ExportToExcel(DateTime? startDate, DateTime? endDate, string branch, string accGroup)
        {
            // Validate at least one filter is provided for export
            if (string.IsNullOrEmpty(accGroup) &&
                !startDate.HasValue &&
                !endDate.HasValue &&
                string.IsNullOrEmpty(branch))
            {
                // Redirect back to index with error message
                TempData["ErrorMessage"] = "Please provide at least one search criteria before exporting";
                return RedirectToAction("BalanceSheetIndex");
            }

            var reportData = _reportService.GetAll(startDate, endDate, branch, accGroup);

            // Filter by exact AccGroup if specified
            if (!string.IsNullOrWhiteSpace(accGroup))
            {
                reportData = reportData.Where(r =>
                    !string.IsNullOrEmpty(r.AccGroup) &&
                    r.AccGroup.Equals(accGroup, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Group the data
            var groupedData = GroupLedgerData(reportData.ToList());

            // Check if data exists
            if (!groupedData.Any())
            {
                TempData["ErrorMessage"] = "No data found for the selected criteria";
                return RedirectToAction("BalanceSheetIndex");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Balance Sheet");

                // Headers
                worksheet.Cell(1, 1).Value = "Account Group";
                worksheet.Cell(1, 2).Value = "Debit Amount";
                worksheet.Cell(1, 3).Value = "Credit Amount";
                worksheet.Cell(1, 4).Value = "Balance Type";

                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                decimal totalDebit = 0;
                decimal totalCredit = 0;

                foreach (var item in groupedData)
                {
                    worksheet.Cell(row, 1).Value = item.AccGroup ?? "N/A";

                    if (item.BalanceIn?.ToUpper() == "DR")
                    {
                        worksheet.Cell(row, 2).Value = item.BalanceOpening ?? 0;
                        worksheet.Cell(row, 3).Value = 0;
                        totalDebit += item.BalanceOpening ?? 0;
                    }
                    else if (item.BalanceIn?.ToUpper() == "CR")
                    {
                        worksheet.Cell(row, 2).Value = 0;
                        worksheet.Cell(row, 3).Value = item.BalanceOpening ?? 0;
                        totalCredit += item.BalanceOpening ?? 0;
                    }
                    else
                    {
                        worksheet.Cell(row, 2).Value = 0;
                        worksheet.Cell(row, 3).Value = 0;
                    }

                    worksheet.Cell(row, 4).Value = item.BalanceIn ?? "NIL";
                    row++;
                }

                // Add totals row
                worksheet.Cell(row, 1).Value = "TOTAL";
                worksheet.Cell(row, 2).Value = totalDebit;
                worksheet.Cell(row, 3).Value = totalCredit;
                worksheet.Cell(row, 4).Value = "-";

                var totalRow = worksheet.Row(row);
                totalRow.Style.Font.Bold = true;
                totalRow.Style.Fill.BackgroundColor = XLColor.LightBlue;

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BalanceSheetReport.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult ExportToPdf(DateTime? startDate, DateTime? endDate, string branch, string accGroup)
        {
            // Validate at least one filter is provided for export
            if (string.IsNullOrEmpty(accGroup) &&
                !startDate.HasValue &&
                !endDate.HasValue &&
                string.IsNullOrEmpty(branch))
            {
                // Redirect back to index with error message
                TempData["ErrorMessage"] = "Please provide at least one search criteria before exporting";
                return RedirectToAction("BalanceSheetIndex");
            }

            var reportData = _reportService.GetAll(startDate, endDate, branch, accGroup);

            // Filter by exact AccGroup if specified
            if (!string.IsNullOrWhiteSpace(accGroup))
            {
                reportData = reportData.Where(r =>
                    !string.IsNullOrEmpty(r.AccGroup) &&
                    r.AccGroup.Equals(accGroup, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Group the data
            var groupedData = GroupLedgerData(reportData.ToList());

            // Check if data exists
            if (!groupedData.Any())
            {
                TempData["ErrorMessage"] = "No data found for the selected criteria";
                return RedirectToAction("BalanceSheetIndex");
            }

            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
                var title = new Paragraph("Balance Sheet Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(title);

                var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.Black);
                var date = new Paragraph($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", dateFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                };
                document.Add(date);

                var table = new PdfPTable(4) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 3f, 2f, 2f, 2f });

                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.White);
                BaseColor headerBackground = new BaseColor(0, 102, 204);

                string[] headers = { "Account Group", "Debit Amount", "Credit Amount", "Balance Type" };
                foreach (var header in headers)
                {
                    var cell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = headerBackground,
                        Padding = 5,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                decimal totalDebit = 0;
                decimal totalCredit = 0;

                foreach (var item in groupedData)
                {
                    table.AddCell(new Phrase(item.AccGroup ?? "N/A", dataFont));

                    if (item.BalanceIn?.ToUpper() == "DR")
                    {
                        table.AddCell(new Phrase((item.BalanceOpening ?? 0).ToString("N2"), dataFont));
                        table.AddCell(new Phrase("0.00", dataFont));
                        totalDebit += item.BalanceOpening ?? 0;
                    }
                    else if (item.BalanceIn?.ToUpper() == "CR")
                    {
                        table.AddCell(new Phrase("0.00", dataFont));
                        table.AddCell(new Phrase((item.BalanceOpening ?? 0).ToString("N2"), dataFont));
                        totalCredit += item.BalanceOpening ?? 0;
                    }
                    else
                    {
                        table.AddCell(new Phrase("0.00", dataFont));
                        table.AddCell(new Phrase("0.00", dataFont));
                    }

                    table.AddCell(new Phrase(item.BalanceIn ?? "NIL", dataFont));
                }

                // Add totals row
                var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.Black);
                BaseColor totalBackground = new BaseColor(240, 240, 240);

                var totalCells = new string[] { "TOTAL", totalDebit.ToString("N2"), totalCredit.ToString("N2"), "-" };
                foreach (var cellValue in totalCells)
                {
                    var cell = new PdfPCell(new Phrase(cellValue, totalFont))
                    {
                        BackgroundColor = totalBackground,
                        Padding = 5,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                return File(ms.ToArray(), "application/pdf", "BalanceSheetReport.pdf");
            }
        }
    }
}