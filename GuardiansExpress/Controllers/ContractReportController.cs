using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services;
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
    public class ContractReportController : Controller
    {
        private readonly IContractReportService _contractReportService;

        public ContractReportController(IContractReportService contractReportService)
        {
            _contractReportService = contractReportService;
        }

        public IActionResult ContractReportIndex()
        {
            // Get branches for dropdown
            var branches = _contractReportService.GetAllBranches();
            ViewBag.BranchList = new SelectList(branches, "BranchMasterId", "BranchName");
            
            // Get unique account heads
            ViewBag.AccHeadList = _contractReportService.GetAllAccHeads();

            // Invoice Types
            ViewBag.InvoiceTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Invoice Type" },
                new SelectListItem { Value = "Monthly", Text = "Monthly" },
                new SelectListItem { Value = "Quarterly", Text = "Quarterly" },
                new SelectListItem { Value = "Yearly", Text = "Yearly" },
                new SelectListItem { Value = "One-Time", Text = "One-Time" }
            };

            // Contract Types
            ViewBag.ContractTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Contract Type" },
                new SelectListItem { Value = "Rental", Text = "Rental" },
                new SelectListItem { Value = "Service", Text = "Service" },
                new SelectListItem { Value = "Maintenance", Text = "Maintenance" },
                new SelectListItem { Value = "Transportation", Text = "Transportation" }
            };

            return View(new List<ContractReportViewModel>());
        }

        [HttpPost]
        public IActionResult Search(int? branchId, string accHead, string referenceName, string invoiceType, string contractType)
        {
            // Validate at least one filter is provided
            if (!branchId.HasValue &&
                string.IsNullOrEmpty(accHead) &&
                string.IsNullOrEmpty(referenceName) &&
                string.IsNullOrEmpty(invoiceType) &&
                string.IsNullOrEmpty(contractType))
            {
                ModelState.AddModelError("", "Please provide at least one search criteria");
                PopulateViewBagData(branchId, accHead, referenceName, invoiceType, contractType);
                return View("ContractReportIndex", new List<ContractReportViewModel>());
            }

            // Get the contract report data
            var contractData = _contractReportService.GetContractReports(branchId, accHead, referenceName, invoiceType, contractType);

            // Repopulate ViewBag data
            PopulateViewBagData(branchId, accHead, referenceName, invoiceType, contractType);

            return View("ContractReportIndex", contractData);
        }

        [HttpGet]
        public IActionResult ExportToExcel(int? branchId, string accHead, string referenceName, string invoiceType, string contractType)
        {
            var contractData = _contractReportService.GetContractReports(branchId, accHead, referenceName, invoiceType, contractType);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Contract Report");

                // Headers
                worksheet.Cell(1, 1).Value = "S.No";
                worksheet.Cell(1, 2).Value = "Branch";
                worksheet.Cell(1, 3).Value = "Client Name";
                worksheet.Cell(1, 4).Value = "Reference Name";
                worksheet.Cell(1, 5).Value = "Invoice Type";
                worksheet.Cell(1, 6).Value = "Contract Type";
                worksheet.Cell(1, 7).Value = "Invoice No";
                worksheet.Cell(1, 8).Value = "Material Description";
                worksheet.Cell(1, 9).Value = "Start Date";
                worksheet.Cell(1, 10).Value = "End Date";
                worksheet.Cell(1, 11).Value = "Contract End Date";
                worksheet.Cell(1, 12).Value = "From Place";
                worksheet.Cell(1, 13).Value = "To Place";
                worksheet.Cell(1, 14).Value = "Vehicle Type";
                worksheet.Cell(1, 15).Value = "Vehicle Size";
                worksheet.Cell(1, 16).Value = "Freight Rate";
                worksheet.Cell(1, 17).Value = "Auto Invoice";
                worksheet.Cell(1, 18).Value = "End Rental";

                // Style header row
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Data rows
                int row = 2;
                int srNo = 1;
                foreach (var item in contractData)
                {
                    worksheet.Cell(row, 1).Value = srNo++;
                    worksheet.Cell(row, 2).Value = item.BranchName ?? "Head Office";
                    worksheet.Cell(row, 3).Value = item.ClientName ?? "-";
                    worksheet.Cell(row, 4).Value = item.ReferenceName ?? "-";
                    worksheet.Cell(row, 5).Value = item.InvoiceType ?? "-";
                    worksheet.Cell(row, 6).Value = item.ContractType ?? "-";                              
                    worksheet.Cell(row, 7).Value = item.InvoiceNo ?? "-";
                    worksheet.Cell(row, 8).Value = item.MaterialDescription ?? "-";
                    worksheet.Cell(row, 9).Value = item.StartDate?.ToString("dd/MM/yyyy") ?? "-";
                    worksheet.Cell(row, 10).Value = item.EndDate?.ToString("dd/MM/yyyy") ?? "-";
                    worksheet.Cell(row, 11).Value = item.ContractEndDate?.ToString("dd/MM/yyyy") ?? "-";
                    worksheet.Cell(row, 12).Value = item.FromPlace ?? "-";
                    worksheet.Cell(row, 13).Value = item.ToPlace ?? "-";
                    worksheet.Cell(row, 14).Value = item.VehicleType ?? "-";
                    worksheet.Cell(row, 15).Value = item.VehicleSize ?? "-";
                    worksheet.Cell(row, 16).Value = item.FreightRate?.ToString("N2") ?? "-";
                    worksheet.Cell(row, 17).Value = item.AutoInvoice == true ? "Yes" : "No";
                    worksheet.Cell(row, 18).Value = item.EndRental == true ? "Yes" : "No";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ContractReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult ExportToPdf(int? branchId, string accHead, string referenceName, string invoiceType, string contractType)
        {
            var contractData = _contractReportService.GetContractReports(branchId, accHead, referenceName, invoiceType, contractType);

            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
                var title = new Paragraph("Contract Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(title);

                // Date
                var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.Black);
                var date = new Paragraph($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", dateFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                };
                document.Add(date);

                // Table
                var table = new PdfPTable(10) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 1f, 2f, 2.5f, 2f, 1.5f, 1.5f, 1.5f, 2.5f, 1.5f, 1.5f });

                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.White);
                BaseColor headerBackground = new BaseColor(0, 102, 204);

                string[] headers = { "S.No", "Branch", "Client", "Reference", "Invoice Type", "Contract Type", "Invoice No", "Material", "Start Date", "Freight Rate" };
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

                var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
                int srNo = 1;
                foreach (var item in contractData)
                {
                    table.AddCell(new PdfPCell(new Phrase(srNo++.ToString(), dataFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(item.BranchName ?? "Head Office", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.ClientName ?? "-", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.ReferenceName ?? "-", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.InvoiceType ?? "-", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.ContractType ?? "-", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.InvoiceNo ?? "-", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.MaterialDescription ?? "-", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.StartDate?.ToString("dd/MM/yyyy") ?? "-", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(item.FreightRate?.ToString("N2") ?? "-", dataFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                document.Add(table);
                document.Close();

                return File(ms.ToArray(), "application/pdf", $"ContractReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
        }

        private void PopulateViewBagData(int? branchId, string accHead, string referenceName, string invoiceType, string contractType)
        {
            // Repopulate dropdowns
            var branches = _contractReportService.GetAllBranches();
            ViewBag.BranchList = new SelectList(branches, "BranchMasterId", "BranchName", branchId);

            ViewBag.AccHeadList = _contractReportService.GetAllAccHeads();

            ViewBag.InvoiceTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Invoice Type" },
                new SelectListItem { Value = "Monthly", Text = "Monthly", Selected = invoiceType == "Monthly" },
                new SelectListItem { Value = "Quarterly", Text = "Quarterly", Selected = invoiceType == "Quarterly" },
                new SelectListItem { Value = "Yearly", Text = "Yearly", Selected = invoiceType == "Yearly" },
                new SelectListItem { Value = "One-Time", Text = "One-Time", Selected = invoiceType == "One-Time" }
            };

            ViewBag.ContractTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Contract Type" },
                new SelectListItem { Value = "Rental", Text = "Rental", Selected = contractType == "Rental" },
                new SelectListItem { Value = "Service", Text = "Service", Selected = contractType == "Service" },
                new SelectListItem { Value = "Maintenance", Text = "Maintenance", Selected = contractType == "Maintenance" },
                new SelectListItem { Value = "Transportation", Text = "Transportation", Selected = contractType == "Transportation" }
            };

            // Pass selected values back to view
            ViewBag.SelectedBranchId = branchId;
            ViewBag.SelectedAccHead = accHead;
            ViewBag.SelectedReferenceName = referenceName;
            ViewBag.SelectedInvoiceType = invoiceType;
            ViewBag.SelectedContractType = contractType;
        }
    }
}