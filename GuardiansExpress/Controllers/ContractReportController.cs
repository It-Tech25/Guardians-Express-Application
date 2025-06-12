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
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Controllers   
{
    public class ContractReportController : Controller
    {
        private readonly IContractReportService _contractService;
        private readonly MyDbContext _context;

        public ContractReportController(IContractReportService contractService, MyDbContext context)
        {
            _contractService = contractService;
            _context = context;
        }

        public IActionResult ContractReportIndex()
        {
            // Get unique values for dropdowns
            ViewBag.UniqueBranchNames = _contractService.GetUniqueBranchNames();
            ViewBag.UniqueReferenceNames = _contractService.GetUniqueReferenceNames();
            ViewBag.UniqueInvoiceTypes = _contractService.GetUniqueInvoiceTypes();
            ViewBag.UniqueContractTypes = _contractService.GetUniqueContractTypes();

            // Populate Report Type dropdown
            ViewBag.ReportTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Type1", Text = "Type 1" },
                new SelectListItem { Value = "Type2", Text = "Type 2" },
                // Add other report types as needed
            };

            // Return empty list initially - data will load only after search
            return View(new List<ContractEntity>());
        }

        [HttpPost]
        public IActionResult Search(string branchName, string referenceName, string invoiceType, string contractType, bool? tempClose)
        {
            // Populate ViewBag data for the view (needed for dropdowns)
            ViewBag.UniqueBranchNames = _contractService.GetUniqueBranchNames();
            ViewBag.UniqueReferenceNames = _contractService.GetUniqueReferenceNames();
            ViewBag.UniqueInvoiceTypes = _contractService.GetUniqueInvoiceTypes();
            ViewBag.UniqueContractTypes = _contractService.GetUniqueContractTypes();

            // Populate Report Type dropdown
            ViewBag.ReportTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Type1", Text = "Type 1" },
                new SelectListItem { Value = "Type2", Text = "Type 2" },
                // Add other report types as needed
            };

            // Validate at least one filter is provided
            //if (string.IsNullOrEmpty(branchName) &&
            //    string.IsNullOrEmpty(referenceName) &&
            //    string.IsNullOrEmpty(invoiceType) &&
            //    string.IsNullOrEmpty(contractType) &&
            //    !tempClose.HasValue)
            //{
            //    ModelState.AddModelError("", "Please provide at least one search criteria");
            //    return View("ContractReportIndex", new List<ContractModel>());
            //}

            // Pass selected values back to view for persistence
            ViewBag.SelectedBranchName = branchName;
            ViewBag.SelectedReferenceName = referenceName;
            ViewBag.SelectedInvoiceType = invoiceType;
            ViewBag.SelectedContractType = contractType;
            ViewBag.SelectedTempClose = tempClose;

            // Get report data with filters
            var reportData = _contractService.GetAll(branchName, referenceName, invoiceType, contractType, tempClose);

            return View("ContractReportIndex", reportData);
        }

        [HttpGet]
        public IActionResult ExportToExcel(string branchName, string referenceName, string invoiceType, string contractType, bool? tempClose)
        {
            // Validate at least one filter is provided for export
            if (string.IsNullOrEmpty(branchName) &&
                string.IsNullOrEmpty(referenceName) &&
                string.IsNullOrEmpty(invoiceType) &&
                string.IsNullOrEmpty(contractType) &&
                !tempClose.HasValue)
            {
                // Redirect back to index with error message
                TempData["ErrorMessage"] = "Please provide at least one search criteria before exporting";
                return RedirectToAction("ContractReportIndex");
            }

            var reportData = _contractService.GetAll(branchName, referenceName, invoiceType, contractType, tempClose);

            // Check if data exists
            if (!reportData.Any())
            {
                TempData["ErrorMessage"] = "No data found for the selected criteria";
                return RedirectToAction("ContractReportIndex");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Contract Report");

                // Column headers
                worksheet.Cell(1, 1).Value = "Contract ID";
                worksheet.Cell(1, 2).Value = "Branch Name";
                worksheet.Cell(1, 3).Value = "Client Name";
                worksheet.Cell(1, 4).Value = "Reference Name";
                worksheet.Cell(1, 5).Value = "Invoice Type";
                worksheet.Cell(1, 6).Value = "Contract Type";
                worksheet.Cell(1, 7).Value = "Invoice No";
                worksheet.Cell(1, 8).Value = "Contract End Date";
                worksheet.Cell(1, 9).Value = "Temp Close";
                worksheet.Cell(1, 10).Value = "Auto Invoice";
                worksheet.Cell(1, 11).Value = "End Rental";

                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                foreach (var item in reportData)
                {
                    worksheet.Cell(row, 1).Value = item.ContractId;
                    worksheet.Cell(row, 2).Value = item.BranchName ?? "";
                    worksheet.Cell(row, 3).Value = item.ClientName ?? "";
                    worksheet.Cell(row, 4).Value = item.ReferenceName ?? "";
                    worksheet.Cell(row, 5).Value = item.InvoiceType ?? "";
                    worksheet.Cell(row, 6).Value = item.ContractType ?? "";
                    worksheet.Cell(row, 8).Value = item.ContractEndDate?.ToString("dd/MM/yyyy") ?? "";
                    worksheet.Cell(row, 9).Value = item.TempClose == true ? "Yes" : "No";
                    worksheet.Cell(row, 10).Value = item.AutoInvoice == true ? "Yes" : "No";
                    worksheet.Cell(row, 11).Value = item.EndRental == true ? "Yes" : "No";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ContractReport.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult ExportToPdf(string branchName, string referenceName, string invoiceType, string contractType, bool? tempClose)
        {
            // Validate at least one filter is provided for export
            if (string.IsNullOrEmpty(branchName) &&
                string.IsNullOrEmpty(referenceName) &&
                string.IsNullOrEmpty(invoiceType) &&
                string.IsNullOrEmpty(contractType) &&
                !tempClose.HasValue)
            {
                // Redirect back to index with error message
                TempData["ErrorMessage"] = "Please provide at least one search criteria before exporting";
                return RedirectToAction("ContractReportIndex");
            }

            var reportData = _contractService.GetAll(branchName, referenceName, invoiceType, contractType, tempClose);

            // Check if data exists
            if (!reportData.Any())
            {
                TempData["ErrorMessage"] = "No data found for the selected criteria";
                return RedirectToAction("ContractReportIndex");
            }

            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
                var title = new Paragraph("Contract Report", titleFont)
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

                // Create table with 8 columns (key columns only for PDF readability)
                var table = new PdfPTable(8) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 1f, 2f, 2f, 2f, 1.5f, 1.5f, 1.5f, 1f });

                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.White);
                BaseColor headerBackground = new BaseColor(0, 102, 204);

                string[] headers = { "ID", "Branch", "Client", "Reference", "Invoice Type", "Contract Type", "End Date", "Status" };
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

                var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                foreach (var item in reportData)
                {
                    table.AddCell(new Phrase(item.ContractId.ToString(), dataFont));
                    table.AddCell(new Phrase(item.BranchName ?? "", dataFont));
                    table.AddCell(new Phrase(item.ClientName ?? "", dataFont));
                    table.AddCell(new Phrase(item.ReferenceName ?? "", dataFont));
                    table.AddCell(new Phrase(item.InvoiceType ?? "", dataFont));
                    table.AddCell(new Phrase(item.ContractType ?? "", dataFont));
                    table.AddCell(new Phrase(item.ContractEndDate?.ToString("dd/MM/yyyy") ?? "", dataFont));
                    table.AddCell(new Phrase(item.TempClose == true ? "Closed" : "Active", dataFont));
                }
               
                document.Add(table);
                document.Close();

                return File(ms.ToArray(), "application/pdf", "ContractReport.pdf");
            }
        }
    }
}