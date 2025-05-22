using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Services.Service;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GuardiansExpress.Controllers
{
    public class FinancialController : Controller
    {
        private readonly IFinanceService _fService;
        private readonly MyDbContext _context;

        // Constructor
        public FinancialController(IFinanceService fService, MyDbContext context)
        {
            _fService = fService;
            _context = context;
        }

        public IActionResult FinanceIndex()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            // Pass user details to the view
            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;
            //ViewBag.UserProfileImage = loggedInUser.ProfileImageUrl; // URL to the user's profile image

            var res = _fService.Get();
            var re = _context.invoice.Where(a => a.IsDeleted == false).ToList();
            var b = _context.Bill.Where(a => a.IsDeleted == false).ToList();

            ViewBag.Billtype = b;
            ViewBag.invoicetype = re;

            return View(res);
        }

        [HttpPost]
        public IActionResult AddFinance(FinancialyearDto fy, string serializedInvoiceData, string serializedBillData)
        {
            // Validate all required fields manually to ensure proper validation
            if (fy.FromYear <= 0 || fy.ToYear <= 0 || fy.StartDate == null || fy.EndDate == null)
            {
                ModelState.AddModelError("", "All required fields must be filled in.");
            }

            // Check model state
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";

                // Reload required ViewBag data before returning the view
                var res = _fService.Get();
                ViewBag.invoicetype = _context.invoice.Where(a => a.IsDeleted == false).ToList();
                ViewBag.Billtype = _context.Bill.Where(a => a.IsDeleted == false).ToList();

                return View("FinanceIndex", res); // Return with validation errors
            }

            // Validate that data strings are not empty
            if (string.IsNullOrEmpty(serializedInvoiceData) || string.IsNullOrEmpty(serializedBillData))
            {
                ModelState.AddModelError("", "Invoice and Bill data must be provided.");
                TempData["ErrorMessage"] = "Invoice and Bill data must be provided.";

                var res = _fService.Get();
                ViewBag.invoicetype = _context.invoice.Where(a => a.IsDeleted == false).ToList();
                ViewBag.Billtype = _context.Bill.Where(a => a.IsDeleted == false).ToList();

                return View("FinanceIndex", res);
            }

            // Deserialize data
            var invoiceData = JsonConvert.DeserializeObject<List<FinancialInvoiceDto>>(serializedInvoiceData);
            var billData = JsonConvert.DeserializeObject<List<FinancialBillDto>>(serializedBillData);

            // Now save only if everything is valid
            var result = _fService.Add(fy, serializedInvoiceData, serializedBillData);
            if (result.statuCode == 1)
            {
                TempData["SuccessMessage"] = "Financial year added successfully.";
                return RedirectToAction("FinanceIndex");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                TempData["ErrorMessage"] = "Something went wrong while saving.";

                var res = _fService.Get();
                ViewBag.invoicetype = _context.invoice.Where(a => a.IsDeleted == false).ToList();
                ViewBag.Billtype = _context.Bill.Where(a => a.IsDeleted == false).ToList();

                return View("FinanceIndex", res);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFinance(FinancialyearDto financialYear, string serializedInvoiceData, string serializedBillData)
        {
            // Validate all required fields manually
            if (financialYear.FromYear <= 0 || financialYear.ToYear <= 0 || financialYear.StartDate == null || financialYear.EndDate == null)
            {
                return Json(new { success = false, message = "All required fields must be filled in." });
            }

            if (ModelState.IsValid)
            {
                var result = await _fService.UpdateFinancialYearAsync(financialYear, serializedInvoiceData, serializedBillData);
                if (result)
                {
                    return Json(new { success = true, message = "Financial year updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to update financial year." });
                }
            }
            return Json(new { success = false, message = "Invalid data." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFinance(int id)
        {
            var result = await _fService.DeleteFinancialYearAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "Financial year deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete financial year." });
            }
        }

        public IActionResult EditFinancialYear(int id)
        {
            var financialYear = _fService.GetFinancialYearById(id);
            var finvoice = _fService.GetFinancialInvoiceById(id);
            var fbill = _fService.GetFinancialBillById(id);
            return Json(new { financialYear, finvoice, fbill });
        }
    }
}