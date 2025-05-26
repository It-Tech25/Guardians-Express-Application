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
namespace GuardiansExpress.Controllers
{
    public class InvoiceRegisterController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IInvoiceRegisterService _invoiceRegisterService;

        public InvoiceRegisterController(MyDbContext context, IInvoiceRegisterService invoiceRegisterService)
        {
            _context = context;
            _invoiceRegisterService = invoiceRegisterService;
        }

        public IActionResult InvoiceRegisterIndex()
        {
            ViewBag.BranchList = GetBranchList();
            ViewBag.TableColumns = GetColumnNames("SalesInvoice");
            return View(new List<InvoiceDTO>());
        }

        [HttpPost]
        public IActionResult Search()
        {
            var selectedColumns = Request.Form["SelectedColumns"].ToList();
            ViewBag.SelectedColumns = selectedColumns;
            ViewBag.BranchList = GetBranchList();
            ViewBag.TableColumns = GetColumnNames("SalesInvoice");

            // Get form values
            var branchId = Request.Form["Branch"];
            var fromDate = Request.Form["FromDate"];
            var toDate = Request.Form["ToDate"];

            // Get results
            var model = _invoiceRegisterService.GetInvoiceRegisterDetails(
                string.IsNullOrEmpty(branchId) ? null : int.Parse(branchId),
                fromDate,
                toDate);

            TempData["InvoiceRegisterData"] = JsonConvert.SerializeObject(model);
            return View("InvoiceRegisterIndex", model);
        }

        public IActionResult ExportToPdf()
        {
            if (TempData["InvoiceRegisterData"] != null)
            {
                var data = JsonConvert.DeserializeObject<List<InvoiceDTO>>(TempData["InvoiceRegisterData"].ToString());
                using (var stream = new MemoryStream())
                {
                    var document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var title = new Paragraph("Invoice Register Report", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(title);
                    document.Add(new Paragraph("\n"));

                    string[] headers = new string[]
                    {
                        "InvoiceId", "BranchName", "InvoiceNo", "InvDate", "InvoiceType",
                        "ClientId", "OrderNo", "OrderDate", "PONo", "PODate", "DueDate",
                        "GrossAmount", "Discount", "Tax", "RoundOff", "NetAmount",
                        "CostCenter", "PaymentTerm", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
                    };

                    int columnCount = headers.Length;
                    int maxColsPerTable = 6;

                    for (int start = 0; start < columnCount; start += maxColsPerTable)
                    {
                        int end = Math.Min(start + maxColsPerTable, columnCount);
                        string[] visibleHeaders = headers.Skip(start).Take(end - start).ToArray();

                        PdfPTable table = new PdfPTable(visibleHeaders.Length) { WidthPercentage = 100 };

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
                        document.Add(new Paragraph("\n"));
                    }

                    document.Close();
                    var content = stream.ToArray();
                    return File(content, "application/pdf", "InvoiceRegisterReport.pdf");
                }
            }

            return RedirectToAction("InvoiceRegisterIndex");
        }

        private string GetPropertyValue(InvoiceDTO item, string propertyName)
        {
            var prop = typeof(InvoiceDTO).GetProperty(propertyName);
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
            if (TempData["InvoiceRegisterData"] != null)
            {
                var data = JsonConvert.DeserializeObject<List<InvoiceDTO>>(TempData["InvoiceRegisterData"].ToString());
                var columnNames = GetColumnNames("SalesInvoice");

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Invoice Register Report");

                    worksheet.Cells[1, 1].Value = "Invoice Register Report";
                    worksheet.Cells[1, 1, 1, columnNames.Count].Merge = true;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 16;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    for (int i = 0; i < columnNames.Count; i++)
                    {
                        worksheet.Cells[3, i + 1].Value = columnNames[i];
                        worksheet.Cells[3, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[3, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[3, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

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

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    var stream = new MemoryStream();
                    package.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InvoiceRegisterReport.xlsx");
                }
            }

            return RedirectToAction("InvoiceRegisterIndex");
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

        private List<SelectListItem> GetBranchList()
        {
            return _context.branch
                .OrderBy(b => b.BranchName)
                .Select(b => new SelectListItem
                {
                    Value = b.BranchName.ToString(),
                    Text = b.BranchName
                })
                .ToList();
        }
    }
}