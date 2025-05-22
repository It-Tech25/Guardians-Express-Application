using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace GuardiansExpress.Controllers
{
    public class InvoiceTypeController : Controller
    {
        private readonly IInvoiceTypeService _invoiceTypeService;
        private readonly MyDbContext _context;

        public InvoiceTypeController(IInvoiceTypeService invoiceTypeService, MyDbContext context)
        {
            _invoiceTypeService = invoiceTypeService;
            _context = context;
        }

        public IActionResult InvoiceTypeIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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

            var query = _invoiceTypeService.GetInvoiceTypes();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(it => it.InvoiceType.Contains(searchTerm));
            }

            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var invoiceTypes = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var invoiceTypeDTOs = invoiceTypes
                .Where(it => it.IsDeleted == false)
                .Select(it => new InvoiceTypeModel
                {
                    Id = it.Id,
                    InvoiceType = it.InvoiceType,
                    Status = it.Status
                })
                .ToList();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;

            return View(invoiceTypeDTOs);
        }

        [HttpPost]
        public IActionResult CreateInvoiceType(InvoiceTypeMasterEntity entity)
        {
            // Check if this is an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Check for duplicate invoice type before creating
                bool duplicateExists = _context.invoice
                    .Any(it => it.InvoiceType.Trim().ToLower() == entity.InvoiceType.Trim().ToLower() &&
                         it.IsDeleted == false);

                if (duplicateExists)
                {
                    return Json(new { success = false, message = "Invoice Type already exists." });
                }

                var response = _invoiceTypeService.CreateInvoiceType(entity);

                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }

                return Json(new { success = false, message = response.message });
            }
            else
            {
                // Handle non-AJAX request if needed
                bool duplicateExists = _context.invoice
                    .Any(it => it.InvoiceType.Trim().ToLower() == entity.InvoiceType.Trim().ToLower() &&
                         it.IsDeleted == false);

                if (duplicateExists)
                {
                    TempData["ErrorMessage"] = "Invoice Type already exists.";
                    return RedirectToAction("InvoiceTypeIndex");
                }

                var response = _invoiceTypeService.CreateInvoiceType(entity);

                if (response.statuCode == 1)
                {
                    TempData["SuccessMessage"] = response.message;
                }
                else
                {
                    TempData["ErrorMessage"] = response.message;
                }

                return RedirectToAction("InvoiceTypeIndex");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateInvoiceType(InvoiceTypeMasterEntity entity)
        {
            // Check for duplicate invoice type before updating
            bool duplicateExists = _context.invoice
                .Any(it => it.InvoiceType.Trim().ToLower() == entity.InvoiceType.Trim().ToLower() &&
                     it.Id != entity.Id &&
                     it.IsDeleted == false);

            if (duplicateExists)
            {
                return Json(new { success = false, message = "Another Invoice Type with this name already exists." });
            }

            var response = _invoiceTypeService.UpdateInvoiceType(entity);

            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = response.message });
            }

            return Json(new { success = false, message = response.message });
        }

        [HttpPost]
        public IActionResult DeleteInvoiceType(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Invoice Type ID.");
            }

            var response = _invoiceTypeService.DeleteInvoiceType(id);

            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = response.message });
        }
    }
}