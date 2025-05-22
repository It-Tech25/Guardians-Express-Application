using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using GuardiansExpress.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;  
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GuardiansExpress.Controllers
{
    public class BankReconciliationReportController : Controller
    {
        private readonly IBankRecoService _bankRecoService;
        private readonly MyDbContext _context;

        public BankReconciliationReportController(IBankRecoService bankRecoService, MyDbContext context)
        {
            _bankRecoService = bankRecoService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BankReconciliationReportIndex()
        {
            // Fetch distinct bank names from ledgerEntity
            var bankNames = _context.ledgerEntity
                .Where(a => a.IsDeleted == false && !string.IsNullOrEmpty(a.BankName))
                .Select(a => a.BankName)
                .Distinct()
                .ToList();

            ViewBag.BankNames = new SelectList(bankNames); // ✅ Use SelectList for dropdown binding

            // Optional: Session check
            var user = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (user == null)
                return RedirectToAction("Login", "Authenticate");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string bankName, DateTime onDate)
        {
            var report = await _bankRecoService.GetReportAsync(bankName, onDate);
            return PartialView("_BankRecoResult", report);
        }



        [HttpPost]
        public async Task<IActionResult> ExportToExcel(string bankName, DateTime onDate)
        {
            var data = await _bankRecoService.GetReportAsync(bankName, onDate);

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("BankReco");
                ws.Cells["A1"].Value = "Bank Reconciliation Summary";
                ws.Cells["A2"].Value = "Bank Name";
                ws.Cells["B2"].Value = data.BankName;
                ws.Cells["A3"].Value = "On Date";
                ws.Cells["B3"].Value = data.OnDate.ToShortDateString();
                ws.Cells["A4"].Value = "Balance As Per Books";
                ws.Cells["B4"].Value = data.BalanceAsPerBooks;
                ws.Cells["A5"].Value = "Less - Deposited Not Cleared";
                ws.Cells["B5"].Value = data.DepositedButNotCleared;
                ws.Cells["A6"].Value = "Add - Issued Not Cleared";
                ws.Cells["B6"].Value = data.IssuedButNotCleared;
                ws.Cells["A7"].Value = "Balance As Per Bank";
                ws.Cells["B7"].Value = data.BalanceAsPerBank;

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BankReconciliation.xlsx");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ExportToPdf(string bankName, DateTime onDate)
        {
            var data = await _bankRecoService.GetReportAsync(bankName, onDate);

            var doc = new Document();
            var stream = new MemoryStream();
            PdfWriter.GetInstance(doc, stream).CloseStream = false;
            doc.Open();

            doc.Add(new Paragraph("Bank Reconciliation Summary"));
            doc.Add(new Paragraph($"Bank Name: {data.BankName}"));
            doc.Add(new Paragraph($"On Date: {data.OnDate:dd-MM-yyyy}"));
            doc.Add(new Paragraph($"Balance As Per Our Books: {data.BalanceAsPerBooks:N2} Dr"));
            doc.Add(new Paragraph($"Less - Deposited Not Cleared: {data.DepositedButNotCleared:N2}"));
            doc.Add(new Paragraph($"Add - Issued Not Cleared: {data.IssuedButNotCleared:N2}"));
            doc.Add(new Paragraph($"Balance As Per Bank Books: {data.BalanceAsPerBank:N2} Dr"));

            doc.Close();
            stream.Position = 0;

            return File(stream, "application/pdf", "BankReconciliation.pdf");
        }
    }
}
