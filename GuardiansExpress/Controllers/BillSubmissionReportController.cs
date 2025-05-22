using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services;
using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class BillSubmissionReportController : Controller
{
    private readonly IBillSubmissionReportService _reportService;
    private readonly MyDbContext _context;


    public BillSubmissionReportController(IBillSubmissionReportService reportService, MyDbContext context)
    {
        _reportService = reportService;
        _context = context;
    }

    public IActionResult BillSubmissionReportIndex()
    {
        var allColumns = GetColumnNames("BilSubmission");
        ViewBag.TableColumns = allColumns;
        ViewBag.SelectedColumns = allColumns; 
        return View();
    }

    [HttpPost]
    public IActionResult Search(string ClientName, string BillNo)
    {
        var allColumns = GetColumnNames("BilSubmission");
        var selectedColumns = Request.Form["SelectedColumns"].ToList();
        ViewBag.TableColumns = allColumns;
        ViewBag.SelectedColumns = selectedColumns.Count > 0 ? selectedColumns : allColumns;

        var reportData = _context.BilSubmissions
            .Where(b => b.IsDelete==false)
            .Where(b => string.IsNullOrEmpty(ClientName) || b.ClientName.Contains(ClientName))
            .Where(b => string.IsNullOrEmpty(BillNo) || b.BillNo.Contains(BillNo))
            .Select(b => new BillSubmissionReportDTO
            {
                BillSubmissionId = b.BillSubmissionId,
                ClientName = b.ClientName,
                BillNo = b.BillNo,
                BillSubDate = b.BillSubDate,
                BillSubmissionBy = b.BillSubmissionBy,
                ReceivedBy = b.ReceivedBy,
                HandedOverBy = b.HandedOverBy,
                DocketNo = b.DocketNo,
                CourierName = b.CourierName,
                IsActive = b.IsActive == true
            })
            .ToList();

        return View("BillSubmissionReportIndex", reportData);
    }

    [HttpGet]
    public IActionResult ExportToExcel(string ClientName, string BillNo)
    {
        var reportData = _context.BilSubmissions
            .Where(b => !b.IsDelete==false)
            .Where(b => string.IsNullOrEmpty(ClientName) || b.ClientName.Contains(ClientName))
            .Where(b => string.IsNullOrEmpty(BillNo) || b.BillNo.Contains(BillNo))
            .Select(b => new BillSubmissionReportDTO
            {
                BillSubmissionId = b.BillSubmissionId,
                ClientName = b.ClientName,
                BillNo = b.BillNo,
                BillSubDate = b.BillSubDate,
                BillSubmissionBy = b.BillSubmissionBy,
                ReceivedBy = b.ReceivedBy,
                HandedOverBy = b.HandedOverBy,
                DocketNo = b.DocketNo,
                CourierName = b.CourierName,
                IsActive = b.IsActive == true
            })
            .ToList();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Bill Submissions");

            worksheet.Cell(1, 1).Value = "Bill No";
            worksheet.Cell(1, 2).Value = "Bill Date";
            worksheet.Cell(1, 3).Value = "Client Name";
            worksheet.Cell(1, 4).Value = "Submitted By";
            worksheet.Cell(1, 5).Value = "Received By";
            worksheet.Cell(1, 6).Value = "Handed Over By";
            worksheet.Cell(1, 7).Value = "Docket No";
            worksheet.Cell(1, 8).Value = "Courier Name";

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var item in reportData)
            {
                worksheet.Cell(row, 1).Value = item.BillNo;
                worksheet.Cell(row, 2).Value = item.BillSubDate?.ToString("dd/MM/yyyy") ?? "N/A";
                worksheet.Cell(row, 3).Value = item.ClientName;
                worksheet.Cell(row, 4).Value = item.BillSubmissionBy;
                worksheet.Cell(row, 5).Value = item.ReceivedBy;
                worksheet.Cell(row, 6).Value = item.HandedOverBy;
                worksheet.Cell(row, 7).Value = item.DocketNo ?? "N/A";
                worksheet.Cell(row, 8).Value = item.CourierName ?? "N/A";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BillSubmissionReport.xlsx");
            }
        }
    }

    [HttpGet]
    public IActionResult ExportToPdf(string ClientName, string BillNo)
    {
        var reportData = _context.BilSubmissions
            .Where(b => !b.IsDelete==false)
            .Where(b => string.IsNullOrEmpty(ClientName) || b.ClientName.Contains(ClientName))
            .Where(b => string.IsNullOrEmpty(BillNo) || b.BillNo.Contains(BillNo))
            .Select(b => new BillSubmissionReportDTO
            {
                BillSubmissionId = b.BillSubmissionId,
                ClientName = b.ClientName,
                BillNo = b.BillNo,
                BillSubDate = b.BillSubDate,
                BillSubmissionBy = b.BillSubmissionBy,
                ReceivedBy = b.ReceivedBy,
                HandedOverBy = b.HandedOverBy,
                DocketNo = b.DocketNo,
                CourierName = b.CourierName,
                IsActive = b.IsActive == true
            })
            .ToList();

        using (var ms = new MemoryStream())
        {
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
            var title = new Paragraph("Bill Submission Report", titleFont)
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

            var table = new PdfPTable(8) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 3f, 3f, 4f, 3f, 3f, 3f, 3f, 3f });

            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.White);
            BaseColor headerBackground = new BaseColor(0, 102, 204);

            string[] headers = { "Bill No", "Bill Date", "Client Name", "Submitted By", "Received By", "Handed Over By", "Docket No", "Courier Name" };
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
                table.AddCell(new Phrase(item.BillNo));
                table.AddCell(new Phrase(item.BillSubDate?.ToString("dd/MM/yyyy") ?? "N/A"));
                table.AddCell(new Phrase(item.ClientName));
                table.AddCell(new Phrase(item.BillSubmissionBy));
                table.AddCell(new Phrase(item.ReceivedBy));
                table.AddCell(new Phrase(item.HandedOverBy));
                table.AddCell(new Phrase(item.DocketNo ?? "N/A"));
                table.AddCell(new Phrase(item.CourierName ?? "N/A"));
            }

            document.Add(table);
            document.Close();

            return File(ms.ToArray(), "application/pdf", "BillSubmissionReport.pdf");
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
