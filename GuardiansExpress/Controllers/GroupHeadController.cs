using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GuardiansExpress.Controllers
{
    public class GroupHeadController : Controller
    {
        private readonly IGroupHeadService _groupHeadService;

        public GroupHeadController(IGroupHeadService groupHeadService)
        {
            _groupHeadService = groupHeadService;
        }

        public IActionResult GroupHeadIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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
            // Fetching group head data via service layer with pagination and search
            var groupHeads = _groupHeadService.GetGroupHeads(searchTerm, pageNumber, pageSize).ToList();

            // Pagination details
            int totalRecords = groupHeads.Count();
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(groupHeads);
        }

        // GroupHead Add View
        public IActionResult GroupHeadAdd()
        {
            return View();
        }

        // POST: Add GroupHead
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GroupHeadAdd(GroupHeadModel groupHeadModel)
        {
            if (ModelState.IsValid)
            {
                var response = _groupHeadService.CreateGroupHead(groupHeadModel);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            return View(groupHeadModel);
        }

        // Edit GroupHead View
        public IActionResult GroupHeadEdit(int id)
        {
            var groupHead = _groupHeadService.GetGroupHeadById(id);

            if (groupHead == null)
            {
                return NotFound();
            }

            return View(groupHead);
        }

        // POST: Update GroupHead
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GroupHeadUpdate(GroupHeadModel groupHeadModel)
        {
            if (ModelState.IsValid)
            {
                var response = _groupHeadService.UpdateGroupHead(groupHeadModel);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                return Json(new { success = false, message = response.message });
            }
            return View(groupHeadModel);
        }

        // Delete GroupHead
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Group Head ID.");
            }

            var response = _groupHeadService.DeleteGroupHead(id);

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
