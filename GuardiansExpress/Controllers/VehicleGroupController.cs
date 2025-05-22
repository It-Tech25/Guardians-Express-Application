using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Controllers
{
    public class VehicleGroupController : Controller
    {
        private readonly IVehicleGroupService _vehicleGroupService;

        // Constructor injection for VehicleGroupService
        public VehicleGroupController(IVehicleGroupService vehicleGroupService)
        {
            _vehicleGroupService = vehicleGroupService;
        }

        //----------------------------- Get All Vehicle Groups -------------------------------------------
        public IActionResult VehicleGroupIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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
            // Call service to get the vehicle groups with pagination and search term
            var vehicleGroups = _vehicleGroupService.GetVehicleGroupMaster(searchTerm, pageNumber, pageSize).ToList();

            // Get total count after filtering (You may want to refactor this to return the total count from the service)
            int totalRecords = vehicleGroups.Count();

            // Set view data
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(vehicleGroups);
        }

        //----------------------------- Add Vehicle Group -------------------------------------------
        [HttpPost]
        public IActionResult VehicleGroupAdd(VehicleGroupModel vehicleGroupModel)
        {
            if (ModelState.IsValid)
            {
                // Call service to create a new vehicle group
                var response = _vehicleGroupService.CreateVehicleGroupMaster(vehicleGroupModel);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            return Json(new { success = false, message = "Invalid data" });
        }

        //----------------------------- Edit Vehicle Group (GET) -------------------------------------------
        public IActionResult VehicleGroupEdit(int id)
        {
            // Retrieve the Vehicle Group by its ID for editing
            var vehicleGroup = _vehicleGroupService.GetVehicleGroupMasterById(id);

            if (vehicleGroup == null)
            {
                return NotFound();
            }

            return View(vehicleGroup);
        }

        //----------------------------- Edit Vehicle Group (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VehicleGroupUpdate(VehicleGroupModel vehicleGroupModel)
        {
            if (ModelState.IsValid)
            {
                var response = _vehicleGroupService.UpdateVehicleGroupMaster(vehicleGroupModel);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                return Json(new { success = false, message = response.message });
            }
            return Json(new { success = false, message = "Invalid data" });
        }

        //----------------------------- Delete Vehicle Group -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return BadRequest("Invalid Vehicle Group ID.");
            }

            // Call service to delete the vehicle group
            var response = _vehicleGroupService.DeleteVehicleGroupMaster(id);

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
