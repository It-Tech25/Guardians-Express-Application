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

namespace GuardiansExpress.Controllers
{
    public class GrReportController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IGRReportService _grReportService;

        public GrReportController(MyDbContext context, IGRReportService grReportService)
        {
            _context = context;
            _grReportService = grReportService;
        }

        public IActionResult GRReportIndex()
        {
            ViewBag.BranchList = _context.branch.Select(x => new SelectListItem { Value = x.id.ToString(), Text = x.BranchName }).ToList();
            ViewBag.GRTypeList = _context.gRTypes.Select(x => new SelectListItem { Value = x.GRTypeId.ToString(), Text = x.TypeName }).ToList();
            ViewBag.StatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "All", Text = "All" },
                new SelectListItem { Value = "Active", Text = "Active" },
                new SelectListItem { Value = "Inactive", Text = "Inactive" }
            };

            ViewBag.TableColumns = GetColumnNames("GR");

            return View();
        }

        [HttpPost]
        public IActionResult Search()
        {
            var selectedColumns = Request.Form["SelectedColumns"].ToList();
            ViewBag.SelectedColumns = selectedColumns;

            // Reload dropdowns for view
            ViewBag.BranchList = _context.branch.Select(x => new SelectListItem { Value = x.id.ToString(), Text = x.BranchName }).ToList();
            ViewBag.GRTypeList = _context.gRTypes.Select(x => new SelectListItem { Value = x.GRTypeId.ToString(), Text = x.TypeName }).ToList();
            ViewBag.TableColumns = GetColumnNames("GRMaster");

            // Get form values
            var branchId = Request.Form["Branch"];
            var fromDate = Request.Form["FromDate"];
            var toDate = Request.Form["ToDate"];
            var fromGRNo = Request.Form["FromGRNo"];
            var toGRNo = Request.Form["ToGRNo"];
            var status = Request.Form["Status"];

            // Get results
            var model = _grReportService.Getgrdetails(
                string.IsNullOrEmpty(branchId) ? null : int.Parse(branchId),
                fromDate,
                toDate,
                fromGRNo,
                toGRNo,
                status);
            TempData["GRReportData"] = JsonConvert.SerializeObject(model);
            return View("GRReportIndex", model);
        }

        //    public IActionResult ExportToPdf()
        //    {
        //        if (TempData["GRReportData"] != null)
        //        {
        //            var data = JsonConvert.DeserializeObject<List<GRDTOs>>(TempData["GRReportData"].ToString());
        //            using (var stream = new MemoryStream())
        //            {
        //                var document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
        //                PdfWriter.GetInstance(document, stream);
        //                document.Open();

        //                string[] headers = new string[]
        //                {
        //"GRId", "Branch", "VehicleNo", "OwnedBy", "Grtype", "GRNo", "GRDate",
        //"ClientName", "BillingAddress", "FreightAmount", "GrossWeight", "LoadWeight",
        //"Consigner", "FromPlace", "Consignee", "ToPlace", "Transporter", "HireRate",
        //"ItemDescription", "Quantity", "IncurencedBy", "InsurenceNo", "IsActive", "IsDeleted",
        //"CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        //                };

        //                var table = new PdfPTable(headers.Length) { WidthPercentage = 100 };
        //                table.SetWidths(Enumerable.Repeat(1f, headers.Length).ToArray()); // Optional: evenly space columns

        //                foreach (var header in headers)
        //                {
        //                    table.AddCell(new PdfPCell(new Phrase(header)) { BackgroundColor = BaseColor.LightGray });
        //                }

        //                foreach (var item in data)
        //                {
        //                    table.AddCell(item.GRId.ToString());
        //                    table.AddCell(item.Branch ?? "-");
        //                    table.AddCell(item.VehicleNo ?? "-");
        //                    table.AddCell(item.OwnedBy ?? "-");
        //                    table.AddCell(item.Grtype ?? "-");
        //                    table.AddCell(item.GRNo?.ToString() ?? "-");
        //                    table.AddCell(item.GRDate?.ToString("yyyy-MM-dd") ?? "-");
        //                    table.AddCell(item.ClientName ?? "-");
        //                    table.AddCell(item.BillingAddress?.ToString() ?? "-");
        //                    table.AddCell(item.FreightAmount?.ToString("F2") ?? "-");
        //                    table.AddCell(item.GrossWeight ?? "-");
        //                    table.AddCell(item.LoadWeight ?? "-");
        //                    table.AddCell(item.Consigner ?? "-");
        //                    table.AddCell(item.FromPlace?.ToString() ?? "-");
        //                    table.AddCell(item.Consignee ?? "-");
        //                    table.AddCell(item.ToPlace?.ToString() ?? "-");
        //                    table.AddCell(item.Transporter ?? "-");
        //                    table.AddCell(item.HireRate?.ToString("F2") ?? "-");
        //                    table.AddCell(item.ItemDescription ?? "-");
        //                    table.AddCell(item.Quantity?.ToString() ?? "-");
        //                    table.AddCell(item.IncurencedBy?.ToString() ?? "-");
        //                    table.AddCell(item.InsurenceNo ?? "-");
        //                    table.AddCell(item.IsActive.HasValue ? (item.IsActive.Value ? "Yes" : "No") : "-");
        //                    table.AddCell(item.IsDeleted.HasValue ? (item.IsDeleted.Value ? "Yes" : "No") : "-");
        //                    table.AddCell(item.CreatedOn?.ToString("yyyy-MM-dd") ?? "-");
        //                    table.AddCell(item.CreatedBy?.ToString() ?? "-");
        //                    table.AddCell(item.UpdatedOn?.ToString("yyyy-MM-dd") ?? "-");
        //                    table.AddCell(item.UpdatedBy?.ToString() ?? "-");
        //                }
        //                document.Add(table);
        //                document.Close();
        //                var content = stream.ToArray();
        //                return File(content, "application/pdf", "GRReport.pdf");
        //            }
        //        }
        //        return RedirectToAction("GRReportIndex");
        //    }
        public IActionResult ExportToPdf()
        {
            if (TempData["GRReportData"] != null)
            {
                var data = JsonConvert.DeserializeObject<List<GRDTOs>>(TempData["GRReportData"].ToString());
                using (var stream = new MemoryStream())
                {
                    // Use Portrait mode (remove Rotate)
                    var document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Define column headers
                    string[] headers = new string[]
                    {
                "GRId", "Branch", "VehicleNo", "OwnedBy", "Grtype", "GRNo", "GRDate",
                "ClientName", "BillingAddress", "FreightAmount", "GrossWeight", "LoadWeight",
                "Consigner", "FromPlace", "Consignee", "ToPlace", "Transporter", "HireRate",
                "ItemDescription", "Quantity", "IncurencedBy", "InsurenceNo", "IsActive", "IsDeleted",
                "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
                    };

                    int columnCount = headers.Length;

                    // Reduce number of columns per table to fit portrait layout
                    int maxColsPerTable = 8; // Show 8 columns at a time (you can adjust this)

                    // Generate table groups
                    for (int start = 0; start < columnCount; start += maxColsPerTable)
                    {
                        int end = Math.Min(start + maxColsPerTable, columnCount);
                        string[] visibleHeaders = headers.Skip(start).Take(end - start).ToArray();

                        PdfPTable table = new PdfPTable(visibleHeaders.Length) { WidthPercentage = 100 };

                        // Add headers
                        foreach (var header in visibleHeaders)
                        {
                            var headerCell = new PdfPCell(new Phrase(header))
                            {
                                BackgroundColor = BaseColor.LightGray
                            };
                            table.AddCell(headerCell);
                        }

                        // Add rows
                        foreach (var item in data)
                        {
                            foreach (var header in visibleHeaders)
                            {
                                string value = GetPropertyValue(item, header);
                                table.AddCell(value ?? "-");
                            }
                        }

                        document.Add(table);
                        document.Add(new Paragraph("\n")); // Add space between table parts
                    }

                    document.Close();
                    var content = stream.ToArray();
                    return File(content, "application/pdf", "GRReport_Portrait.pdf");
                }
            }

            return RedirectToAction("GRReportIndex");
        }

        // Helper to get property value by name
        private string GetPropertyValue(GRDTOs item, string propertyName)
        {
            var prop = typeof(GRDTOs).GetProperty(propertyName);
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
var data = JsonConvert.DeserializeObject<List<GRDTOs>>(TempData["GRReportData"].ToString());            var columnNames = GetColumnNames("GR");

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("GR Report");

                // Add headers
                for (int i = 0; i < columnNames.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnNames[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var item in data)
                {
                    int col = 1;
                    foreach (var colName in columnNames)
                    {
                        var prop = item.GetType().GetProperty(colName);
                        var value = prop != null ? prop.GetValue(item) : null;
                        worksheet.Cells[row, col].Value = value ?? "-";
                        col++;
                    }
                    row++;
                }

                // Auto fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GRReport.xlsx");
            }
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