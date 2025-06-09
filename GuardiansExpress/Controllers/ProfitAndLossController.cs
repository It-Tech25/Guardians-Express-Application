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
    public class ProfitAndLossController : Controller
    {
        private readonly IProfitAndLossService _reportService;
        private readonly MyDbContext _context;

        public ProfitAndLossController(IProfitAndLossService reportService, MyDbContext context)
        {
            _reportService = reportService;
            _context = context;
        }

        public IActionResult ProfitAndLossIndex()
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

            // Populate Report Type dropdown
            ViewBag.ReportTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Type1", Text = "Type 1" },
                new SelectListItem { Value = "Type2", Text = "Type 2" },
                // Add other report types as needed
            };

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

            // Populate Report Type dropdown
            ViewBag.ReportTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Type1", Text = "Type 1" },
                new SelectListItem { Value = "Type2", Text = "Type 2" },
                // Add other report types as needed
            };

            // Validate at least one filter is provided
            if (string.IsNullOrEmpty(accGroup) &&
                !startDate.HasValue &&
                !endDate.HasValue &&
                string.IsNullOrEmpty(branch))
            {
                ModelState.AddModelError("", "Please provide at least one search criteria");
                return View("ProfitAndLossIndex", new List<LedgerMasterEntity>());
            }

            // Pass selected values back to view for persistence
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedBranch = branch;
            ViewBag.SelectedAccGroup = accGroup;

            // Get report data with filters
            var reportData = _reportService.GetAll(startDate, endDate, branch, accGroup);

            // Filter by exact AccGroup if specified
            if (!string.IsNullOrWhiteSpace(accGroup))
            {
                reportData = reportData.Where(r =>
                    !string.IsNullOrEmpty(r.AccGroup) &&
                    r.AccGroup.Equals(accGroup, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return View("ProfitAndLossIndex", reportData);
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
                return RedirectToAction("ProfitAndLossIndex");
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

            // Check if data exists
            if (!reportData.Any())
            {
                TempData["ErrorMessage"] = "No data found for the selected criteria";
                return RedirectToAction("ProfitAndLossIndex");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Profit Loss");

                // Column headers
                worksheet.Cell(1, 1).Value = "Acc Group";
                worksheet.Cell(1, 2).Value = "Opening Balance";
                worksheet.Cell(1, 3).Value = "Balance Type";
                worksheet.Cell(1, 4).Value = "Balance";

                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                foreach (var item in reportData)
                {
                    worksheet.Cell(row, 1).Value = item.AccGroup ?? "";
                    worksheet.Cell(row, 2).Value = item.BalanceOpening ?? 0;
                    worksheet.Cell(row, 3).Value = item.BalanceIn ?? "";
                    worksheet.Cell(row, 4).Value = item.BalanceIn ?? "";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProfitAndLossReport.xlsx");
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
                return RedirectToAction("ProfitAndLossIndex");
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

            // Check if data exists
            if (!reportData.Any())
            {
                TempData["ErrorMessage"] = "No data found for the selected criteria";
                return RedirectToAction("ProfitAndLossIndex");
            }

            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
                var title = new Paragraph("Profit And Loss Report", titleFont)
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

                // Create table with 4 columns
                var table = new PdfPTable(4) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 3f, 3f, 3f, 3f });

                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.White);
                BaseColor headerBackground = new BaseColor(0, 102, 204);

                string[] headers = { "Account Group", "Opening Balance", "Balance Type", "Balance" };
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
                foreach (var item in reportData)
                {
                    table.AddCell(new Phrase(item.AccGroup ?? "", dataFont));
                    table.AddCell(new Phrase(item.BalanceOpening?.ToString() ?? "0", dataFont));
                    table.AddCell(new Phrase(item.BalanceIn ?? "", dataFont));
                    table.AddCell(new Phrase(item.BalanceIn ?? "", dataFont));
                }

                document.Add(table);
                document.Close();

                return File(ms.ToArray(), "application/pdf", "ProfitAndLossReport.pdf");
            }
        }
    }
}