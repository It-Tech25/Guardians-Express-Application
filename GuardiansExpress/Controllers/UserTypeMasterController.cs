using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GuardiansExpress.Controllers
{
    public class UserTypeMasterController : Controller
    {
        private readonly IUserTypeMasterService _uService;
        private readonly MyDbContext _context;
        private readonly ILogger<UserTypeMasterController> _logger;

        public UserTypeMasterController(
            MyDbContext context,
            IUserTypeMasterService uService,
            ILogger<UserTypeMasterController> logger)
        {
            _context = context;
            _uService = uService;
            _logger = logger;
        }

        public IActionResult UserTypeIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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

            try
            {
                var query = _uService.GetUserTypeMasters(searchTerm, pageNumber, pageSize);
                int totalRecords = query.Count();
                var userTypes = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Set view data
                ViewData["PageSize"] = pageSize;
                ViewData["SearchTerm"] = searchTerm;
                ViewData["CurrentPage"] = pageNumber;
                ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

                return View(userTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user type masters");
                TempData["ErrorMessage"] = "An error occurred while retrieving user types.";
                return View(new List<UserTypeMasterDto>());
            }
        }

        [HttpGet]
        public IActionResult UserTypeAdd()
        {
            // Return a view for adding a new user type
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserTypeAdd(UserTypeMasterDto userType)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                _logger.LogWarning("Model validation failed: {Errors}", string.Join(", ", errors));

                TempData["ErrorMessage"] = "Please correct the input errors.";
                return View(userType);
            }

            try
            {
                var response = _uService.CreateUserTypeMaster(userType);
                if (response.statuCode == 1)
                {
                    TempData["SuccessMessage"] = "User Type added successfully: " + response.message;
                    return RedirectToAction("UserTypeIndex");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error: " + response.message;
                    return View(userType);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user type master");
                TempData["ErrorMessage"] = "An unexpected error occurred while adding the user type.";
                return View(userType);
            }
        }

        [HttpGet]
        public IActionResult UserTypeUpdate(int id)
        {
            // Fetch the existing user type for editing
            var userType = _uService.GetUserTypeMasterById(id);
            if (userType == null)
            {
                TempData["ErrorMessage"] = "User Type not found.";
                return RedirectToAction(nameof(UserTypeIndex));
            }
            return View(userType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserTypeUpdate(UserTypeMasterDto userType)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the input errors.";
                return View(userType);
            }

            try
            {
                var response = _uService.UpdateUserTypeMaster(userType);
                if (response.statuCode == 1)
                {
                    TempData["SuccessMessage"] = "User Type updated successfully: " + response.message;
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorMessage"] = "Error: " + response.message;
                    return View(userType);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user type master");
                TempData["ErrorMessage"] = "An unexpected error occurred while updating the user type.";
                return View(userType);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid User Type ID.");
            }

            try
            {
                var response = _uService.DeleteUserTypeMaster(id);
                if (response.statuCode == 1)
                {
                    TempData["SuccessMessage"] = "User Type deleted successfully.";
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user type master");
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public IActionResult ClearTempData()
        {
            TempData.Clear();
            return Ok();
        }
    }
}