using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class TaxMasterController : Controller
    {
        private readonly ITaxMasterService _tService;
        private readonly MyDbContext _context;

        // Constructor injection for CompanyService
        public TaxMasterController(MyDbContext context, ITaxMasterService tService)
        {
            _context = context;
            _tService = tService;
        }

        //----------------------------- Get All Tax Masters -------------------------------------------
        public IActionResult TaxIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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
            ViewBag.Ledger = _context.ledgerEntity.Where(a => a.IsDeleted == false && a.IsActive == true).Distinct().ToList();

            var query = _tService.GetTaxMastersWithLedger(searchTerm, pageNumber, pageSize);
            int totalRecords = query.Count();

            var taxMasters = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Set view data for pagination
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewData["TotalRecords"] = totalRecords;

            return View(taxMasters);
        }

        //----------------------------- Add Tax -------------------------------------------
        [HttpPost]
        public IActionResult TaxAdd(TaxMasterDto tax)
        {
            // Server-side validation
            if (string.IsNullOrEmpty(tax.TaxName) || tax.TaxType == "--Select--" || tax.Status == "--Select--")
            {
                return Json(new
                {
                    success = false,
                    statuCode = 0,
                    message = "Please fill in all required fields correctly."
                });
            }

            // Check if this is an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var response = _tService.CreateTaxMaster(tax);

                // Return JSON response with success property that matches what JavaScript expects
                return Json(new
                {
                    success = response.statuCode == 1,
                    statuCode = response.statuCode,
                    message = response.message,
                    currentId = response.currentId
                });
            }
            else
            {
                // Handle non-AJAX request (form submission)
                var response = _tService.CreateTaxMaster(tax);

                if (response.statuCode == 1)
                {
                    TempData["SuccessMessage"] = "Success: " + response.message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Error: " + response.message;
                }

                return RedirectToAction("TaxIndex");
            }
        }

        //----------------------------- Edit Tax -------------------------------------------
        [HttpPost]
        public IActionResult TaxUpdate(TaxMasterDto tax)
        {
            // Server-side validation
            if (string.IsNullOrEmpty(tax.TaxName) || tax.TaxType == "--Select--" || tax.Status == "--Select--")
            {
                return Json(new
                {
                    success = false,
                    statuCode = 0,
                    message = "Please fill in all required fields correctly."
                });
            }

            var response = _tService.UpdateTaxMaster(tax);

            // Always return JSON for consistency with AJAX calls
            return Json(new
            {
                success = response.statuCode == 1,
                statuCode = response.statuCode,
                message = response.message,
                currentId = response.currentId
            });
        }

        //----------------------------- Delete Tax -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return Json(new { success = false, message = "Invalid Tax ID." });
            }

            // Call service to delete the tax
            var response = _tService.DeleteTaxMaster(id);

            return Json(new
            {
                success = response.statuCode == 1,
                statuCode = response.statuCode,
                message = response.message,
                currentId = response.currentId
            });
        }

        [HttpPost]
        public IActionResult ClearTempData()
        {
            TempData.Clear();
            return Ok();
        }
    }
}