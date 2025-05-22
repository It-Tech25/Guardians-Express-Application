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
    public class GroupSummaryReportController : Controller
    {
        private readonly IGroupSummaryReportService _reportService;
        private readonly MyDbContext _context;

        public GroupSummaryReportController(IGroupSummaryReportService reportService, MyDbContext context)
        {
            _reportService = reportService;
            _context = context;
        }

        public IActionResult GroupSummaryReportIndex()
        {
            var allColumns = GetColumnNames("LedgerMaster");
            ViewBag.TableColumns = allColumns;
            ViewBag.SelectedColumns = allColumns;

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

            return View();
        }

        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate, string branch, string accGroup,
    string reportType, string ledger, string agent, string balType, bool withBalance = false, bool showImportant = false)
        {
            // Validate at least one filter is provided
            if (string.IsNullOrEmpty(accGroup) &&
                !startDate.HasValue &&
                !endDate.HasValue &&
                string.IsNullOrEmpty(branch) &&
                string.IsNullOrEmpty(reportType) &&
                string.IsNullOrEmpty(ledger))
            {
                ModelState.AddModelError("", "Please provide at least one search criteria");
                return View("GroupSummaryReportIndex", new List<LedgerMasterEntity>());
            }

            var allColumns = GetColumnNames("LedgerMaster");

            // Fix for the StringValues ambiguity issue
            List<string> selectedColumns;
            if (Request.Form.TryGetValue("SelectedColumns", out var selectedColumnsValues))
            {
                selectedColumns = selectedColumnsValues.ToList();
            }
            else
            {
                selectedColumns = allColumns;
            }

            ViewBag.TableColumns = allColumns;
            ViewBag.SelectedColumns = selectedColumns.Count > 0 ? selectedColumns : allColumns;

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

            // Pass selected values back to view for persistence
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedBranch = branch;
            ViewBag.SelectedAccGroup = accGroup;
            ViewBag.SelectedReportType = reportType;
            ViewBag.SelectedLedger = ledger;
            ViewBag.SelectedBalType = balType;

            // Populate Report Type dropdown
            ViewBag.ReportTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Type1", Text = "Type 1", Selected = reportType == "Type1" },
                new SelectListItem { Value = "Type2", Text = "Type 2", Selected = reportType == "Type2" },
                // Add other report types as needed
            };

            // Get report data with exact AccGroup match
            var reportData = _reportService.GetAll( startDate,  endDate,  branch,  accGroup,
             reportType,  ledger,  agent,  balType,  withBalance = false,  showImportant = false);

            // Filter by exact AccGroup if specified
            if (!string.IsNullOrWhiteSpace(accGroup))
            {
                reportData = reportData.Where(r =>
                    !string.IsNullOrEmpty(r.AccGroup) &&
                    r.AccGroup.Equals(accGroup, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return View("GroupSummaryReportIndex", reportData);
        }

        [HttpGet]
        public IActionResult ExportToExcel(DateTime? startDate, DateTime? endDate, string branch, string accGroup,
           string reportType, string ledger, string agent, string balType, bool withBalance = false, bool showImportant = false)
        {
            var reportData = _reportService.GetAll(startDate, endDate, branch, accGroup,
                reportType, ledger, agent, balType, withBalance, showImportant);

            // Filter by exact AccGroup if specified
            if (!string.IsNullOrWhiteSpace(accGroup))
            {
                reportData = reportData.Where(r =>
                    !string.IsNullOrEmpty(r.AccGroup) &&
                    r.AccGroup.Equals(accGroup, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Group Summary");

                worksheet.Cell(1, 1).Value = "Date";
                worksheet.Cell(1, 2).Value = "Branch";
                worksheet.Cell(1, 3).Value = "Acc Group";
                worksheet.Cell(1, 4).Value = "Report Type";
                worksheet.Cell(1, 5).Value = "Ledger";
                worksheet.Cell(1, 6).Value = "Agent";
                worksheet.Cell(1, 7).Value = "Bal. Type";
                worksheet.Cell(1, 8).Value = "Balance";
                worksheet.Cell(1, 9).Value = "Important";

                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                foreach (var item in reportData)
                {
                    worksheet.Cell(row, 1).Value = item.CreatedOn;
                    worksheet.Cell(row, 2).Value = item.BranchName;
                    worksheet.Cell(row, 3).Value = item.AccGroup;
                    worksheet.Cell(row, 4).Value = item.RegistrationType;
                    worksheet.Cell(row, 5).Value = item.LedgerId;
                    worksheet.Cell(row, 6).Value = item.Agent;
                    worksheet.Cell(row, 7).Value = item.BalanceIn;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GroupSummaryReport.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult ExportToPdf(DateTime? startDate, DateTime? endDate, string branch, string accGroup,
            string reportType, string ledger, string agent, string balType, bool withBalance = false, bool showImportant = false)
        {
            var reportData = _reportService.GetAll(startDate, endDate, branch, accGroup,
                reportType, ledger, agent, balType, withBalance, showImportant);

            // Filter by exact AccGroup if specified
            if (!string.IsNullOrWhiteSpace(accGroup))
            {
                reportData = reportData.Where(r =>
                    !string.IsNullOrEmpty(r.AccGroup) &&
                    r.AccGroup.Equals(accGroup, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
                var title = new Paragraph("Group Summary Report", titleFont)
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

                var table = new PdfPTable(9) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 3f, 3f, 3f, 3f, 3f, 3f, 3f, 3f, 2f });

                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.White);
                BaseColor headerBackground = new BaseColor(0, 102, 204);

                string[] headers = { "Date", "Branch", "Acc Group", "Report Type", "Ledger", "Agent", "Bal. Type", "Balance", "Important" };
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
                    table.AddCell(new Phrase(item.CreatedOn.HasValue ? item.CreatedOn.Value.ToString("dd-MM-yyyy") : "-"));
                    table.AddCell(new Phrase(item.BranchName));
                    table.AddCell(new Phrase(item.AccGroup));
                    table.AddCell(new Phrase(item.RegistrationType));
                    table.AddCell(new Phrase(item.LedgerId));
                    table.AddCell(new Phrase(item.Agent));
                    table.AddCell(new Phrase(item.BalanceIn));
                    // Add other cells as needed
                }

                document.Add(table);
                document.Close();

                return File(ms.ToArray(), "application/pdf", "GroupSummaryReport.pdf");
            }
        }

        public List<string> GetColumnNames(string tableName)
        {
            var columnNames = new List<string>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName";
                var param = command.CreateParameter();
                param.ParameterName = "@tableName";
                param.Value = tableName;
                command.Parameters.Add(param);
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columnNames.Add(reader.GetString(0));
                    }
                }
                _context.Database.CloseConnection();
            }
            return columnNames;
        }
    }
}