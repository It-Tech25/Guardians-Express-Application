using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GuardiansExpress.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly MyDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public VehicleTypeController(IVehicleTypeService vehicleTypeService, MyDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _vehicleTypeService = vehicleTypeService;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult VehicleTypeIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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
            var vehicleTypes = _vehicleTypeService.GetVehicleTypes(searchTerm, pageNumber, pageSize).ToList();
            int totalRecords = vehicleTypes.Count();

            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(vehicleTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VehicleTypeAdd(VehicleTypeDTO vehicleTypeDTO, IFormFile VehicleImage)
        {
            try
            {
                string vehicleImageUrl = ProcessVehicleImage(VehicleImage);

                // Only set the image if one was uploaded
                if (!string.IsNullOrEmpty(vehicleImageUrl) && vehicleImageUrl != "dummy.png")
                {
                    vehicleTypeDTO.VehicleImage = "/VehicleImage/" + vehicleImageUrl;
                }

                var response = _vehicleTypeService.CreateVehicleType(vehicleTypeDTO);

                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // Add this method - it's required by your JavaScript
        [HttpGet]
        public IActionResult GetVehicleTypeDetails(int id)
        {
            try
            {
                var vehicleType = _vehicleTypeService.GetVehicleTypeById(id);

                if (vehicleType == null)
                {
                    return Json(new { success = false, message = "Vehicle type not found." });
                }

                return Json(new
                {
                    success = true,
                    id = vehicleType.Id,
                    vehicleType = vehicleType.VehicleType,
                    vehicleImage = vehicleType.VehicleImage,
                    isActive = vehicleType.IsActive
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VehicleTypeUpdate(VehicleTypeDTO vehicleTypeDTO, IFormFile VehicleImage)
        {
            try
            {
                // Get existing vehicle type to preserve image if not updating
                var existingVehicleType = _vehicleTypeService.GetVehicleTypeById(vehicleTypeDTO.Id);

                if (existingVehicleType == null)
                {
                    return Json(new { success = false, message = "Vehicle type not found." });
                }

                // Only process image if a new one was uploaded
                if (VehicleImage != null && VehicleImage.Length > 0)
                {
                    string vehicleImageUrl = ProcessVehicleImage(VehicleImage);
                    vehicleTypeDTO.VehicleImage = "/VehicleImage/" + vehicleImageUrl;
                }
                else
                {
                    // Keep existing image
                    vehicleTypeDTO.VehicleImage = existingVehicleType.VehicleImage;
                }

                var response = _vehicleTypeService.UpdateVehicleType(vehicleTypeDTO);

                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating vehicle type: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(new { success = false, message = "Invalid Vehicle Type ID." });
            }

            var response = _vehicleTypeService.DeleteVehicleType(id);

            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        // Helper method to process vehicle images
        private string ProcessVehicleImage(IFormFile vehicleImage)
        {
            if (vehicleImage == null || vehicleImage.Length == 0)
            {
                return "dummy.png";
            }

            try
            {
                var vehicleImageFileName = Path.GetFileName(vehicleImage.FileName);
                if (string.IsNullOrEmpty(vehicleImageFileName))
                {
                    return "dummy.png";
                }

                string filename = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                filename = Regex.Replace(filename, "[A-Za-z ]", "");
                filename += RandomGenerator.RandomString(4, false);
                string extension = Path.GetExtension(vehicleImageFileName);
                filename += extension;

                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "VehicleImage");

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var filePath = Path.Combine(uploads, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vehicleImage.CopyTo(fileStream);
                }

                return filename;
            }
            catch (Exception)
            {
                return "dummy.png";
            }
        }
    }
}