using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GuardiansExpress.Controllers
{
    public class GRTypeController : Controller
    {
        private readonly IGRTypeService _grTypeService;
        private readonly MyDbContext _context;

        public GRTypeController(IGRTypeService grTypeService, MyDbContext context)
        {
            _grTypeService = grTypeService;
            _context = context;
        }

        //----------------------------- Get All GR Types -------------------------------------------
        public IActionResult GRTypeIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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
            // Debugging: Check the searchTerm value
            Console.WriteLine($"Search Term: {searchTerm}");

            var query = _context.gRTypes
                        .Where(g => g.IsDeleted == false &&
                                   (string.IsNullOrEmpty(searchTerm) || g.TypeName.ToLower().Contains(searchTerm.ToLower())));

            int totalRecords = query.Count();

            var grTypes = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(grTypes);
        }

        //----------------------------- Add GR Type -------------------------------------------
        [HttpPost]
        public IActionResult GRTypeAdd(GRType grType)
        {
            var response = _grTypeService.AddGRType(grType);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = response.message });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        //----------------------------- Edit GR Type (GET) -------------------------------------------
        public IActionResult GREdit(int id)
        {
            var grType = _grTypeService.GetGRType(id);

            if (grType == null)
            {
                return NotFound();
            }

            return View(grType);
        }

        //----------------------------- Edit GR Type (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GRUpdate(GRType model)
        {
            var response = _grTypeService.EditGRType(model);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = response.message });
            }
            return Json(new { success = false, message = response.message });
        }

        //----------------------------- Delete GR Type -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid GR Type ID.");
            }

            var response = _grTypeService.RemoveGRType(id);

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

// Let me know if you’d like me to fine-tune this or add anything extra! 🚀
