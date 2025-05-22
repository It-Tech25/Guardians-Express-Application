using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;    
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using GuardiansExpress.Services.Interfaces;
using Newtonsoft.Json;
using OfficeOpenXml;
using GuardiansExpress.Services.Service;   
namespace GuardiansExpress.Controllers
{
    public class ExpCreditNoteController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IExpCreditNoteService _expCreditNoteService;

        public ExpCreditNoteController(MyDbContext context, IExpCreditNoteService expCreditNoteService)
        {
            _context = context;
            _expCreditNoteService = expCreditNoteService;
        }

        public IActionResult ExpCreditNoteIndex()
        {
            ViewBag.BranchList = _context.branch.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.BranchName
            }).ToList();

            ViewBag.TableColumns = GetColumnNames("ExpCreditNote");

            return View();
        }

        [HttpPost]
        public IActionResult Search()
        {
            var selectedColumns = Request.Form["SelectedColumns"].ToList();
            ViewBag.SelectedColumns = selectedColumns;

            // Reload dropdowns for view
            ViewBag.BranchList = _context.branch.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.BranchName
            }).ToList();

            ViewBag.TableColumns = GetColumnNames("EXP_CREDITNote"); 

            // Get form values
            var branchId = Request.Form["Branch"];
            var fromDate = Request.Form["FromDate"];
            var toDate = Request.Form["ToDate"];

            // Get results
            var model = _expCreditNoteService.GetExpCreditNoteDetails(
                string.IsNullOrEmpty(branchId) ? null : int.Parse(branchId),
                fromDate,
                toDate);

            TempData["ExpCreditNoteData"] = JsonConvert.SerializeObject(model);
            return View("ExpCreditNoteIndex", model);
        }

        public IActionResult ExportToPdf()
        {
            if (TempData["ExpCreditNoteData"] != null)
            {
                var data = JsonConvert.DeserializeObject<List<Exp_credit>>(TempData["ExpCreditNoteData"].ToString());
                using (var stream = new MemoryStream())
                {
                    // Use Portrait mode
                    var document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Add title
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var title = new Paragraph("Exp/Credit Note Register", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(title);
                    document.Add(new Paragraph("\n"));

                    // Define column headers
                    string[] headers = new string[]
                    {
                        "ID", "Branch", "Note No", "Note Date", "Note Type", "Amount",
                        "Description", "Reference No", "Party Name", "Created On",
                        "Created By", "Updated On", "Updated By"
                    };

                    int columnCount = headers.Length;
                    int maxColsPerTable = 6; // Show 6 columns at a time for better readability

                    // Generate table groups
                    for (int start = 0; start < columnCount; start += maxColsPerTable)
                    {
                        int end = Math.Min(start + maxColsPerTable, columnCount);
                        string[] visibleHeaders = headers.Skip(start).Take(end - start).ToArray();

                        PdfPTable table = new PdfPTable(visibleHeaders.Length) { WidthPercentage = 100 };

                        // Add headers
                        foreach (var header in visibleHeaders)
                        {
                            var headerCell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)))
                            {
                                BackgroundColor = BaseColor.LightGray,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            };
                            table.AddCell(headerCell);
                        }

                        // Add rows
                        foreach (var item in data)
                        {
                            foreach (var header in visibleHeaders)
                            {
                                string value = GetPropertyValue(item, header);
                                var cell = new PdfPCell(new Phrase(value ?? "-", FontFactory.GetFont(FontFactory.HELVETICA, 8)))
                                {
                                    Padding = 3
                                };
                                table.AddCell(cell);
                            }
                        }

                        document.Add(table);
                        document.Add(new Paragraph("\n")); // Add space between table parts
                    }

                    document.Close();
                    var content = stream.ToArray();
                    return File(content, "application/pdf", "ExpCreditNoteReport.pdf");
                }
            }

            return RedirectToAction("ExpCreditNoteIndex");
        }

        // Helper to get property value by name
        private string GetPropertyValue(Exp_credit item, string propertyName)
        {
            var prop = typeof(Exp_credit).GetProperty(propertyName);
            var value = prop?.GetValue(item, null);

            if (value == null)
                return "-";

            if (value is DateTime dt)
                return dt.ToString("yyyy-MM-dd");

            if (value is bool b)
                return b ? "Yes" : "No";

            if (value is decimal dec)
                return dec.ToString("F2");

            return value.ToString();
        }

        public IActionResult ExportToExcel()
        {
            if (TempData["ExpCreditNoteData"] != null)
            {
                var data = JsonConvert.DeserializeObject<List<Exp_credit>>(TempData["ExpCreditNoteData"].ToString());
                var columnNames = GetColumnNames("EXP_CREDITNote");

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Exp Credit Note Report");

                    // Add title
                    worksheet.Cells[1, 1].Value = "";
                    worksheet.Cells[1, 1].Merge = true;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 16;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Add headers
                    for (int i = 0; i < columnNames.Count; i++)
                    {
                        worksheet.Cells[3, i + 1].Value = columnNames[i];
                        worksheet.Cells[3, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[3, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[3, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    // Add data
                    int row = 4;
                    foreach (var item in data)
                    {
                        int col = 1;
                        foreach (var colName in columnNames)
                        {
                            var prop = item.GetType().GetProperty(colName);
                            var value = prop != null ? prop.GetValue(item) : null;
                            worksheet.Cells[row, col].Value = value ?? "-";
                            worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            col++;
                        }
                        row++;
                    }

                    // Auto fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    var stream = new MemoryStream();
                    package.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExpCreditNoteReport.xlsx");
                }
            }

            return RedirectToAction("ExpCreditNoteIndex");
        }

        private List<string> GetColumnNames(string tableName)
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