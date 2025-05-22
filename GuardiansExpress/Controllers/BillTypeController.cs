using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GuardiansExpress.Controllers
{
    public class BillTypeController : Controller
    {
        private readonly IBillTypeService _billTypeService;
        private readonly MyDbContext _context;

        // Constructor injection for BillTypeService
        public BillTypeController(IBillTypeService billTypeService, MyDbContext context)
        {
            _billTypeService = billTypeService;
            _context = context;
        }

        //----------------------------- Get All Bill Types -------------------------------------------

        //public IActionResult BillTypeIndex(string searchTerm = "")
        //{
        //    // Retrieve the list of BillTypes
        //    IEnumerable<BillEntity> billTypes = _billTypeService.GetBillMaster();

        //    // If searchTerm is not empty, filter the group heads by name or behaviour
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        billTypes = billTypes.Where(g => g.BillType.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //                                            g.Status.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //                                            g.Id.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        //                                            );

        //    }

        //    // Pass the searchTerm to the view using ViewData
        //    ViewData["SearchTerm"] = searchTerm;
        //    return View(billTypes);
        //}



        public IActionResult BillTypeIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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
            // Retrieve the list of bill types
            var billTypesQuery = _billTypeService.GetBillMaster().AsQueryable();

           
            int totalRecords = billTypesQuery.Count();

            // Apply pagination
            var billTypes = billTypesQuery.Skip((pageNumber - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToList();

            // Calculate total pages based on total records
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Set view data for pagination and search term
            ViewData["SearchTerm"] = searchTerm;
            ViewData["PageSize"] = pageSize;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            // Return the paginated and filtered list of bill types to the view
            return View(billTypes);
        }





        //----------------------------- Add Bill Type -------------------------------------------
        [HttpPost]
        public IActionResult BillTypeAdd(BillEntity bill)
        {
            // Call service to create a new BillType
            var response = _billTypeService.CreateBillMaster(bill);
            if (response.statuCode == 1)
            {
                return RedirectToAction("BillTypeIndex");
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        //----------------------------- Edit Bill Type (GET) -------------------------------------------
        public IActionResult BillTypeEdit(int id)
        {
            // Retrieve the BillType by its ID for editing
            var bill = _billTypeService.GetBillById(id);

            if (bill == null)
            {
                return NotFound();
            }

            return Json(new { success = true });
        }

        //----------------------------- Edit Bill Type (POST) -------------------------------------------
        
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult BillTypeUpdate(BillEntity model)
        {
            
                var response = _billTypeService.UpdateBillMaster(model);

            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else {
                return Json(new { success = false, message = response.message }); }

            }
            



        //----------------------------- Delete Bill Type -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return BadRequest("Invalid Bill Type ID.");
            }

            // Call service to delete the BillType
            var response = _billTypeService.DeleteBillMaster(id);

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
