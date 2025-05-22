using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GuardiansExpress.Controllers
{
    public class CompanySetUpController : Controller
    {
        private readonly ICompanySetupService _companyService;
        private readonly MyDbContext _context;

        // Constructor injection for CompanyService
        public CompanySetUpController(ICompanySetupService companyService,MyDbContext context)
        {
            _companyService = companyService;
            _context = context;
        }

        //----------------------------- Get All Companies -------------------------------------------
        
        public IActionResult CompanySetUpIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            LoginResponse lr = new LoginResponse();
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
            var query = _context.CompanySetups
                        .Where(a => a.IsDeleted == false &&
                                   (string.IsNullOrEmpty(searchTerm) || a.CompanyName.Contains(searchTerm)));

            // Get total count after filtering
            int totalRecords = query.Count();

            // Apply pagination
            var companies = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Set view data
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(companies);
        }

        //----------------------------- Add Company -------------------------------------------
        [HttpPost]
        public IActionResult CompanyAdd(CompanySetupMasterEntity company)
        {
            // Call service to create a new company
            var response = _companyService.CreateCompanySetupMaster(company);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = response.message });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        //----------------------------- Edit Company (GET) -------------------------------------------
        public IActionResult CompanyEdit(int id)
        {
            // Retrieve the Company by its ID for editing
            var company = _companyService.GetCompanySetupMasterById(id);

            if (company == null)
            {
                return NotFound();
            }

            return Json(new { success = true });
        }

        //----------------------------- Edit Company (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompanyUpdate(CompanySetupMasterEntity model)
        {
            var response = _companyService.UpdateCompanySetupMaster(model);
            if (response.statuCode == 1)
            {
                return RedirectToAction("CompanySetUpIndex");
            }
            return Json(new { success = false, message = response.message });
        }

        //----------------------------- Delete Company -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return BadRequest("Invalid Company ID.");
            }

            // Call service to delete the company
            var response = _companyService.DeleteCompanySetupMaster(id);

            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }
    }
}
