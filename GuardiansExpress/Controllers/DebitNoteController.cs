using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Controllers
{
    public class DebitNoteController : Controller
    {
        private readonly IDebitNoteService _debitNoteService;
        private readonly MyDbContext _context;

        public DebitNoteController(IDebitNoteService debitNoteService, MyDbContext context)
        {
            _debitNoteService = debitNoteService;
            _context = context;
        }

        // GET: DebitNote/Filter
        public async Task<IActionResult> Filter(DebitNoteFilterDTO filter)
        {
            ViewBag.BranchList=_context.branch.Where(a=>a.IsDeleted==false).Select(a=>a.BranchName).ToList();
            var results = _debitNoteService.GetByFilterAsync(filter);
            return View(results); // Ensure your view expects IEnumerable<DebitNoteFilterDTO>
        }


        [HttpPost]
        public IActionResult DebitNoteFilterIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.creditNotes
                .Where(a => a.IsDeleted==false &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             a.DN_CN_No.Contains(searchTerm) ||
                             a.BillNo.Contains(searchTerm) ||
                             a.Branch.Contains(searchTerm) ||
                             a.AccHead.Contains(searchTerm)));

            int totalRecords = query.Count();

            var creditNotes = query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var creditNoteModels = creditNotes.Select(MapToModel).ToList();

            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(creditNoteModels); // Ensure your view expects IEnumerable<CreditNoteModel>
        }

        // Export to PDF or Excel
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Export(DebitNoteFilterDTO filter, string exportType)
        {
            var debitNotes = _debitNoteService.GetByFilterAsync(filter);

            if (exportType == "Excel")
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("DebitNotes");

                    // Header
                    worksheet.Cell(1, 1).Value = "DN_CN_No";
                    worksheet.Cell(1, 2).Value = "Note Date";
                    worksheet.Cell(1, 3).Value = "Branch";
                    worksheet.Cell(1, 4).Value = "Note Type";
                    worksheet.Cell(1, 5).Value = "Account Head";
                    worksheet.Cell(1, 6).Value = "Bill No";
                    worksheet.Cell(1, 7).Value = "Bill Date";
                    worksheet.Cell(1, 8).Value = "Sales Type";
                    worksheet.Cell(1, 9).Value = "Bill Amount";
                    worksheet.Cell(1, 10).Value = "Select Address";
                    worksheet.Cell(1, 11).Value = "GSTIN";
                    worksheet.Cell(1, 12).Value = "Address";
                    worksheet.Cell(1, 13).Value = "No GST";
                    worksheet.Cell(1, 14).Value = "Is Deleted";
                    worksheet.Cell(1, 15).Value = "Is Active";
                    worksheet.Cell(1, 16).Value = "Updated By";
                    worksheet.Cell(1, 17).Value = "Created At";
                    worksheet.Cell(1, 18).Value = "Updated At";

                    int row = 2;
                    foreach (var note in debitNotes)
                    {
                        worksheet.Cell(row, 1).Value = note.DN_CN_No;
                        worksheet.Cell(row, 2).Value = note.NoteDate?.ToString("yyyy-MM-dd");
                        worksheet.Cell(row, 3).Value = note.Branch;
                        worksheet.Cell(row, 4).Value = note.NoteType;
                        worksheet.Cell(row, 5).Value = note.AccHead;
                        worksheet.Cell(row, 6).Value = note.BillNo;
                        worksheet.Cell(row, 7).Value = note.BillDate?.ToString("yyyy-MM-dd");
                        worksheet.Cell(row, 8).Value = note.SalesType;
                        worksheet.Cell(row, 9).Value = note.BillAmount ?? 0;
                        worksheet.Cell(row, 10).Value = note.SelectAddress;
                        worksheet.Cell(row, 11).Value = note.AccGSTIN;
                        worksheet.Cell(row, 12).Value = note.Address;
                        worksheet.Cell(row, 13).Value = note.NoGST.HasValue && note.NoGST.Value ? "Yes" : "No";
                        worksheet.Cell(row, 14).Value = note.IsDeleted.HasValue && note.IsDeleted.Value ? "Yes" : "No";
                        worksheet.Cell(row, 15).Value = note.IsActive.HasValue && note.IsActive.Value ? "Yes" : "No";
                        worksheet.Cell(row, 16).Value = note.UpdatedBy;
                        worksheet.Cell(row, 17).Value = note.CreatedAt?.ToString("yyyy-MM-dd");
                        worksheet.Cell(row, 18).Value = note.UpdatedAt?.ToString("yyyy-MM-dd");
                        row++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "DebitNotes.xlsx");
                    }
                }
            }
            else if (exportType == "PDF")
            {
                using (var stream = new MemoryStream())
                {
                    var document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    var table = new PdfPTable(18) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

                    // Headers
                    string[] headers = {
                "DN_CN_No", "Note Date", "Branch", "Note Type", "Account Head",
                "Bill No", "Bill Date", "Sales Type", "Bill Amount", "Select Address",
                "GSTIN", "Address", "No GST", "Is Deleted", "Is Active",
                "Updated By", "Created At", "Updated At"
            };

                    foreach (var header in headers)
                        table.AddCell(new Phrase(header, FontFactory.GetFont("Arial", 8, Font.BOLD)));

                    foreach (var note in debitNotes)
                    {
                        table.AddCell(note.DN_CN_No ?? "");
                        table.AddCell(note.NoteDate?.ToString("yyyy-MM-dd") ?? "");
                        table.AddCell(note.Branch ?? "");
                        table.AddCell(note.NoteType ?? "");
                        table.AddCell(note.AccHead ?? "");
                        table.AddCell(note.BillNo ?? "");
                        table.AddCell(note.BillDate?.ToString("yyyy-MM-dd") ?? "");
                        table.AddCell(note.SalesType ?? "");
                        table.AddCell(note.BillAmount?.ToString() ?? "0");
                        table.AddCell(note.SelectAddress ?? "");
                        table.AddCell(note.AccGSTIN ?? "");
                        table.AddCell(note.Address ?? "");
                        table.AddCell(note.NoGST.HasValue && note.NoGST.Value ? "Yes" : "No");
                        table.AddCell(note.IsDeleted.HasValue && note.IsDeleted.Value ? "Yes" : "No");
                        table.AddCell(note.IsActive.HasValue && note.IsActive.Value ? "Yes" : "No");
                        table.AddCell(note.UpdatedBy ?? "");
                        table.AddCell(note.CreatedAt?.ToString("yyyy-MM-dd") ?? "");
                        table.AddCell(note.UpdatedAt?.ToString("yyyy-MM-dd") ?? "");
                    }

                    document.Add(table);
                    document.Close();

                    var content = stream.ToArray();
                    return File(content, "application/pdf", "DebitNotes.pdf");
                }
            }

            return RedirectToAction("Filter", filter);
        }

        // Helper to map entity to model
        private CreditNoteModel MapToModel(CreditNoteEntity entity)
        {
            return new CreditNoteModel
            {
                Id = entity.Id,
                Branch = entity.Branch,
                NoteDate = entity.NoteDate,
                NoteType = entity.NoteType,
                DN_CN_No = entity.DN_CN_No,
                AccHead = entity.AccHead,
                BillNo = entity.BillNo,
                BillDate = entity.BillDate,
                SalesType = entity.SalesType,
                BillAmount = entity.BillAmount,
                SelectAddress = entity.SelectAddress,
                AccGSTIN = entity.AccGSTIN,
                Address = entity.Address,
                NoGST = entity.NoGST,
                IsDeleted = entity.IsDeleted,
                IsActive = entity.IsActive,
                UpdatedBy = entity.UpdatedBy,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
