using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Controllers
{
    public class SubGroupHeadController : Controller
    {
        private readonly ISubGroupHeadService _subGroupHeadService;

        public SubGroupHeadController(ISubGroupHeadService subGroupHeadService)
        {
            _subGroupHeadService = subGroupHeadService;
        }

        //----------------------------- Get All Sub Group Heads -------------------------------------------
        [HttpGet]
        public IActionResult SubGroupHeadIndex(string searchTerm = "")
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
            IEnumerable<SubGroupEntity> subGroupHeads = _subGroupHeadService.GetSubGroupHeadMaster();
            IEnumerable<SubGroupHeadDTO> subGroup = _subGroupHeadService.SubGroupHeadList();
            IEnumerable<GroupHeadEntity> grouphead = _subGroupHeadService.GroupHeadMaster();
            ViewBag.GroupHead = grouphead;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                subGroupHeads = subGroupHeads.Where(s => s.SubGroupName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                                                        s.GroupId.ToString().Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                                                        s.subgroupId.ToString().Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase));
            }

            ViewData["SearchTerm"] = searchTerm;
            return View(subGroup);
        }

        //----------------------------- Add Sub Group Head -------------------------------------------
        [HttpPost]
        public IActionResult SubGroupHeadAdd(SubGroupEntity subGroupHead)
        {
            if (subGroupHead == null)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            var response = _subGroupHeadService.CreateSubGroupHeadMaster(subGroupHead);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = "SubGroup added successfully." });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        //----------------------------- Edit Sub Group Head (GET) -------------------------------------------
        [HttpGet]  // Added attribute for clarity
        public IActionResult SubGroupHeadEdit(int id)
        {
            var subGroupHead = _subGroupHeadService.GetSubGroupHeadById(id);
            if (subGroupHead == null)
            {
                return NotFound();
            }

            return View(subGroupHead);
        }

        //----------------------------- Edit Sub Group Head (POST) -------------------------------------------
        [HttpPost]
        public IActionResult SubGroupHeadUpdate(SubGroupEntity subGroupHead)
        {
            if (subGroupHead == null)
            {
                return Json(new { success = false, message = "Invalid Sub Group Head Data." });
            }

            var response = _subGroupHeadService.UpdateSubGroupHeadMaster(subGroupHead);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = "SubGroup updated successfully." });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }
        //----------------------------- Delete Sub Group Head -------------------------------------------
        [HttpPost]
        public IActionResult SubGroupHeadDelete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Sub Group Head ID.");
            }

            var response = _subGroupHeadService.DeleteSubGroupHeadMaster(id);
            if (response.statuCode == 1) // Fixed typo
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = response.message }); // Fixed property casing
            }
        }
    }
}
