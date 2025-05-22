using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class VehicleStatusController : Controller
    {
        private readonly IVehicleStatusService _vehicleStatusService;

        // Constructor injection for VehicleStatusService
        public VehicleStatusController(IVehicleStatusService vehicleStatusService)
        {
            _vehicleStatusService = vehicleStatusService;
        }

        //----------------------------- Get All Vehicle Statuses -------------------------------------------
        public IActionResult VehicleStatusIndex(string searchTerm = "")
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
            // Retrieve the list of VehicleStatuses
            IEnumerable<VehicleStatusEntity> vehicleStatuses = _vehicleStatusService.GetAllVehicleStatuses();

            // If searchTerm is not empty, filter the vehicle statuses by name or ID
            if (!string.IsNullOrEmpty(searchTerm))
            {
                vehicleStatuses = vehicleStatuses.Where(v => v.VehicleStatusName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || v.VehicleStatusID.ToString() == searchTerm);
            }

            return View(vehicleStatuses);
        }

        //----------------------------- Add Vehicle Status (GET) -------------------------------------------
        public IActionResult VehicleStatusAdd()
        {
            return View();
        }

        //----------------------------- Add Vehicle Status (POST) -------------------------------------------
        [HttpPost]
        public IActionResult VehicleStatusAdd(VehicleStatusEntity vehicleStatusModel)
        {
            // Call service to create a new VehicleStatus
            var response = _vehicleStatusService.AddVehicleStatus(vehicleStatusModel);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = response.message });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        //----------------------------- Edit Vehicle Status (GET) --------------------------------------------
        public IActionResult VehicleStatusEdit(int id)
        {
            // Retrieve the VehicleStatus by its ID for editing
            var vehicleStatus = _vehicleStatusService.GetVehicleStatus(id);

            if (vehicleStatus == null)



            {
                return NotFound();
            }

            return View(vehicleStatus);
        }

        //----------------------------- Edit Vehicle Status (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VehicleStatusUpdate(VehicleStatusEntity model)
        {
            // Call service to update the existing VehicleStatus
            var response = _vehicleStatusService.UpdateVehicleStatus(model);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = response.message });
            }
            return Json(new { success = false, message = response.message });
        }

        //----------------------------- Delete Vehicle Status -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return BadRequest("Invalid Vehicle Status ID.");
            }

            // Call service to delete the VehicleStatus
            var response = _vehicleStatusService.RemoveVehicleStatus(id);

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
