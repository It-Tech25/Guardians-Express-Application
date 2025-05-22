using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services;
using GuardiansExpress.Models.Entity;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardiansExpress.Controllers
{
    public class DayBookController : Controller
    {
        private readonly IDayBookService _dayBookService;
        private readonly MyDbContext _context;

        public DayBookController(IDayBookService dayBookService, MyDbContext context)
        {
            _dayBookService = dayBookService;
            _context = context;
        }

        public IActionResult DayBookIndex()
        {
            // Get all branches for dropdown
            ViewBag.Branches = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.Branch)
                .Distinct()
                .OrderBy(b => b)
                .Select(b => new SelectListItem { Value = b, Text = b })
                .ToList();

            // Add "All" option to branches
            ViewBag.Branches.Insert(0, new SelectListItem { Value = "All", Text = "All", Selected = true });

            // Get transaction types for dropdown
            ViewBag.TransactionTypes = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.FromTransactionType)
                .Distinct()
                .OrderBy(t => t)
                .Select(t => new SelectListItem { Value = t, Text = t })
                .ToList();

            // Add "All" option to transaction types
            ViewBag.TransactionTypes.Insert(0, new SelectListItem { Value = "All", Text = "All", Selected = true });

            // Get account heads for datalist
            ViewBag.AccountHeads = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.AccountHead)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            // Balance types dropdown
            ViewBag.BalanceTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select", Selected = true },
                new SelectListItem { Value = "Dr", Text = "Dr" },
                new SelectListItem { Value = "Cr", Text = "Cr" }
            };

            // TxnTypes dropdown from voucher table
            ViewBag.TxnTypes = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.FromTransactionType)
                .Distinct()
                .OrderBy(t => t)
                .Select(t => new SelectListItem { Value = t, Text = t })
                .ToList();

            ViewBag.TxnTypes.Insert(0, new SelectListItem { Value = "All", Text = "All", Selected = true });

            // Book types dropdown
            ViewBag.BookTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select", Selected = true },
                new SelectListItem { Value = "GR", Text = "GR" },
                new SelectListItem { Value = "Sale", Text = "Sale" },
                new SelectListItem { Value = "Voucher", Text = "Voucher" },
                new SelectListItem { Value = "Purchase", Text = "Purchase" }
            };

            // Default to today's date for both start and end date
            ViewBag.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.EndDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Define default selected columns
            ViewBag.SelectedColumns = new List<string>
            {
                "SrNo", "Date", "ReferenceNo", "AccountHead",
                "Particulars", "VoucherNo", "ChequeNo", "TxnType", "BalanceAmount"
            };

            return View("DayBookIndex", new List<DayBookDTO>());
        }

        [HttpPost]
        public IActionResult Search(DateTime startDate, DateTime endDate, string branch = "All",
            string transactionType = "All", string accHead = "", string balType = "", string bookType = "", string txnType = "All", int? recordsPerPage = 5)
        {
            // Validate dates
            if (startDate > endDate)
            {
                ModelState.AddModelError("", "Start date cannot be after end date");
                return RedirectToAction("DayBookIndex");
            }

            // Get data based on filters
            var dayBookData = _dayBookService.GetDayBookEntries(startDate, endDate, branch,
                transactionType, accHead, balType, bookType, txnType);

            // Add Sr. No. to the data
            int srNo = 1;
            foreach (var item in dayBookData)
            {
                item.SrNo = srNo++;

                // Set TxnType if not already set by the service
                if (string.IsNullOrEmpty(item.TxnType))
                {
                    // Determine TxnType based on DebitAmount and CreditAmount
                    if (item.DebitAmount > 0)
                    {
                        item.TxnType = "Dr";
                        item.BalanceAmount = item.DebitAmount.ToString("N2");
                    }
                    else if (item.CreditAmount > 0)
                    {
                        item.TxnType = "Cr";
                        item.BalanceAmount = item.CreditAmount.ToString("N2");
                    }
                    else
                    {
                        item.TxnType = "-";
                        item.BalanceAmount = "0.00";
                    }
                }
            }

            // Persist filter values for the view
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.ToString("yyyy-MM-dd");
            ViewBag.SelectedBranch = branch;
            ViewBag.SelectedTransactionType = transactionType;
            ViewBag.SelectedAccHead = accHead;
            ViewBag.SelectedBalType = balType;
            ViewBag.SelectedBookType = bookType;
            ViewBag.SelectedTxnType = txnType;
            ViewBag.RecordsPerPage = recordsPerPage;

            // Define selected columns
            ViewBag.SelectedColumns = new List<string>
            {
                "SrNo", "Date", "ReferenceNo", "AccountHead",
                "Particulars", "VoucherNo", "ChequeNo", "TxnType", "BalanceAmount"
            };

            // Get all branches for dropdown
            ViewBag.Branches = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.Branch)
                .Distinct()
                .OrderBy(b => b)
                .Select(b => new SelectListItem { Value = b, Text = b, Selected = b == branch })
                .ToList();

            // Add "All" option to branches
            ViewBag.Branches.Insert(0, new SelectListItem { Value = "All", Text = "All", Selected = branch == "All" });

            // Get transaction types for dropdown
            ViewBag.TransactionTypes = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.VoucherType)
                .Distinct()
                .OrderBy(t => t)
                .Select(t => new SelectListItem { Value = t, Text = t, Selected = t == transactionType })
                .ToList();

            // Add "All" option to transaction types
            ViewBag.TransactionTypes.Insert(0, new SelectListItem { Value = "All", Text = "All", Selected = transactionType == "All" });

            // TxnTypes dropdown from voucher table
            ViewBag.TxnTypes = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.FromTransactionType)
                .Distinct()
                .OrderBy(t => t)
                .Select(t => new SelectListItem { Value = t, Text = t, Selected = t == txnType })
                .ToList();

            ViewBag.TxnTypes.Insert(0, new SelectListItem { Value = "All", Text = "All", Selected = txnType == "All" });

            // Get account heads for datalist
            ViewBag.AccountHeads = _context.Vouchers
                .Where(v => v.IsDelete == false && v.IsActive == true)
                .Select(v => v.AccountHead)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            // Balance types dropdown
            ViewBag.BalanceTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select", Selected = string.IsNullOrEmpty(balType) },
                new SelectListItem { Value = "Dr", Text = "Dr", Selected = balType == "Dr" },
                new SelectListItem { Value = "Cr", Text = "Cr", Selected = balType == "Cr" }
            };

            // Book types dropdown
            ViewBag.BookTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select", Selected = string.IsNullOrEmpty(bookType) },
                new SelectListItem { Value = "GR", Text = "GR", Selected = bookType == "GR" },
                new SelectListItem { Value = "Sale", Text = "Sale", Selected = bookType == "Sale" },
                new SelectListItem { Value = "Voucher", Text = "Voucher", Selected = bookType == "Voucher" },
                new SelectListItem { Value = "Purchase", Text = "Purchase", Selected = bookType == "Purchase" }
            };

            return View("DayBookIndex", dayBookData);
        }

        [HttpGet]
        public IActionResult ExportToExcel(DateTime startDate, DateTime endDate, string branch = "All",
            string transactionType = "All", string accHead = "", string balType = "", string bookType = "", string txnType = "All")
        {
            // Get data based on filters
            var dayBookData = _dayBookService.GetDayBookEntries(startDate, endDate, branch,
                transactionType, accHead, balType, bookType, txnType);

            // Add Sr. No. to the data and set TxnType and BalanceAmount
            int srNo = 1;
            foreach (var item in dayBookData)
            {
                item.SrNo = srNo++;

                // Set TxnType if not already set by the service
                if (string.IsNullOrEmpty(item.TxnType))
                {
                    // Determine TxnType based on DebitAmount and CreditAmount
                    if (item.DebitAmount > 0)
                    {
                        item.TxnType = "Dr";
                        item.BalanceAmount = item.DebitAmount.ToString("N2");
                    }
                    else if (item.CreditAmount > 0)
                    {
                        item.TxnType = "Cr";
                        item.BalanceAmount = item.CreditAmount.ToString("N2");
                    }
                    else
                    {
                        item.TxnType = "-";
                        item.BalanceAmount = "0.00";
                    }
                }
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Day Book");

                // Set header row
                worksheet.Cell(1, 1).Value = "Sr No.";
                worksheet.Cell(1, 2).Value = "Date";
                worksheet.Cell(1, 3).Value = "Ref. No.";
                worksheet.Cell(1, 4).Value = "Acc Head";
                worksheet.Cell(1, 5).Value = "Particulars";
                worksheet.Cell(1, 6).Value = "Voucher No";
                worksheet.Cell(1, 7).Value = "Cheque No";
                worksheet.Cell(1, 8).Value = "TxnType";
                worksheet.Cell(1, 9).Value = "Balance Amount";

                // Style header row
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Add data rows
                int row = 2;
                foreach (var item in dayBookData)
                {
                    worksheet.Cell(row, 1).Value = item.SrNo;
                    worksheet.Cell(row, 2).Value = item.Date.ToString("dd-MM-yyyy");
                    worksheet.Cell(row, 3).Value = item.ReferenceNo ?? "";
                    worksheet.Cell(row, 4).Value = item.AccountHead ?? "";
                    worksheet.Cell(row, 5).Value = item.Particulars ?? "";
                    worksheet.Cell(row, 6).Value = item.VoucherNo ?? "";
                    worksheet.Cell(row, 7).Value = item.ChequeNo ?? "";
                    worksheet.Cell(row, 8).Value = item.TxnType ?? "";
                    worksheet.Cell(row, 9).Value = item.BalanceAmount ?? "0.00";
                    row++;
                }

                // Add totals row
                worksheet.Cell(row, 7).Value = "Total";
                worksheet.Cell(row, 7).Style.Font.Bold = true;

                // Calculate total based on TxnType
                decimal totalDr = dayBookData.Where(d => d.TxnType == "Dr").Sum(d => d.DebitAmount);
                decimal totalCr = dayBookData.Where(d => d.TxnType == "Cr").Sum(d => d.CreditAmount);

                worksheet.Cell(row, 9).Value = (totalDr + totalCr).ToString("N2");
                worksheet.Cell(row, 9).Style.Font.Bold = true;

                // Format currency cells
                worksheet.Range(2, 9, row, 9).Style.NumberFormat.Format = "#,##0.00";

                // Adjust column widths
                worksheet.Columns().AdjustToContents();

                // Generate Excel file
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"DayBook_{startDate:dd-MM-yyyy}_to_{endDate:dd-MM-yyyy}.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult ExportToPdf(DateTime startDate, DateTime endDate, string branch = "All",
            string transactionType = "All", string accHead = "", string balType = "", string bookType = "", string txnType = "All")
        {
            // Get data based on filters
            var dayBookData = _dayBookService.GetDayBookEntries(startDate, endDate, branch,
                transactionType, accHead, balType, bookType, txnType);

            // Add Sr. No. to the data and set TxnType and BalanceAmount
            int srNo = 1;
            foreach (var item in dayBookData)
            {
                item.SrNo = srNo++;

                // Set TxnType if not already set by the service
                if (string.IsNullOrEmpty(item.TxnType))
                {
                    // Determine TxnType based on DebitAmount and CreditAmount
                    if (item.DebitAmount > 0)
                    {
                        item.TxnType = "Dr";
                        item.BalanceAmount = item.DebitAmount.ToString("N2");
                    }
                    else if (item.CreditAmount > 0)
                    {
                        item.TxnType = "Cr";
                        item.BalanceAmount = item.CreditAmount.ToString("N2");
                    }
                    else
                    {
                        item.TxnType = "-";
                        item.BalanceAmount = "0.00";
                    }
                }
            }

            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Add title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
                var title = new Paragraph("Day Book Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f
                };
                document.Add(title);

                // Add filter information
                var filterFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.DarkGray);
                string filterText = $"Date: {startDate:dd-MM-yyyy} to {endDate:dd-MM-yyyy}";

                if (branch != "All")
                    filterText += $" | Branch: {branch}";

                if (transactionType != "All")
                    filterText += $" | Transaction Type: {transactionType}";

                if (txnType != "All")
                    filterText += $" | TxnType: {txnType}";

                if (!string.IsNullOrEmpty(accHead))
                    filterText += $" | Account Head: {accHead}";

                if (!string.IsNullOrEmpty(balType))
                    filterText += $" | Balance Type: {balType}";

                if (!string.IsNullOrEmpty(bookType))
                    filterText += $" | Book Type: {bookType}";

                var filterInfo = new Paragraph(filterText, filterFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(filterInfo);

                // Create table
                var table = new PdfPTable(9) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 5f, 12f, 12f, 15f, 18f, 12f, 12f, 8f, 12f });

                // Add table headers
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.White);
                BaseColor headerBackground = new BaseColor(0, 102, 204);

                string[] headers = { "Sr No.", "Date", "Ref. No.", "Acc Head", "Particulars", "Voucher No", "Cheque No", "TxnType", "Balance Amount" };
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

                // Add data rows
                var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                foreach (var item in dayBookData)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.SrNo.ToString(), dataFont)) { Padding = 4 });
                    table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("dd-MM-yyyy"), dataFont)) { Padding = 4 });
                    table.AddCell(new PdfPCell(new Phrase(item.ReferenceNo ?? "", dataFont)) { Padding = 4 });
                    table.AddCell(new PdfPCell(new Phrase(item.AccountHead ?? "", dataFont)) { Padding = 4 });
                    table.AddCell(new PdfPCell(new Phrase(item.Particulars ?? "", dataFont)) { Padding = 4 });
                    table.AddCell(new PdfPCell(new Phrase(item.VoucherNo ?? "", dataFont)) { Padding = 4 });
                    table.AddCell(new PdfPCell(new Phrase(item.ChequeNo ?? "", dataFont)) { Padding = 4 });
                    table.AddCell(new PdfPCell(new Phrase(item.TxnType ?? "", dataFont)) { Padding = 4 });

                    var balanceCell = new PdfPCell(new Phrase(item.BalanceAmount ?? "0.00", dataFont))
                    {
                        Padding = 4,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(balanceCell);
                }

                // Add totals row
                var totalLabelCell = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)))
                {
                    Colspan = 8,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 5
                };
                table.AddCell(totalLabelCell);

                // Calculate total based on TxnType
                decimal totalDr = dayBookData.Where(d => d.TxnType == "Dr").Sum(d => d.DebitAmount);
                decimal totalCr = dayBookData.Where(d => d.TxnType == "Cr").Sum(d => d.CreditAmount);

                var totalCell = new PdfPCell(new Phrase((totalDr + totalCr).ToString("N2"),
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 5
                };
                table.AddCell(totalCell);

                document.Add(table);
                document.Close();

                return File(ms.ToArray(), "application/pdf", $"DayBook_{startDate:dd-MM-yyyy}_to_{endDate:dd-MM-yyyy}.pdf");
            }
        }
    }
}