using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Services.Service;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GuardiansExpress.Controllers
{
    public class UserMasterController : Controller
    {
        private readonly IUserMgmtService _userMgmtService;
        private readonly MyDbContext _dbContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UserMasterController(IUserMgmtService _userService, MyDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _userMgmtService = _userService;
            _dbContext = context;
            _hostingEnvironment = hostingEnvironment;

        }
       public IActionResult UserMaster(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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

        // Get paginated results
        var users = _userMgmtService.UserMasterEntity(searchTerm, pageNumber, pageSize);
        
        // Get total count
        int totalRecords =users.Count();

        ViewBag.UserTypes = _dbContext.userTypes
            .Where(x => x.IsDeleted == false && x.IsActive == true)
            .ToList();

        // Set view data for pagination
        ViewData["PageSize"] = pageSize;
        ViewData["SearchTerm"] = searchTerm;
        ViewData["CurrentPage"] = pageNumber;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

        return View(users);
    }
        [HttpGet]
        public IActionResult UserMasterById(int id)
        {
            UsersMasterDTO obj = new UsersMasterDTO();
            var res = _userMgmtService.UserMasterById(id);
            obj = res;
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserTypeAdd(UserMaster user, IFormFile AadharCardFrontUrl, IFormFile AadharCardBackUrl)
        {
            try
            {
                // Process front image with unique filename
                if (AadharCardFrontUrl != null && AadharCardFrontUrl.Length > 0)
                {
                    string frontImageName = $"{Guid.NewGuid()}{Path.GetExtension(AadharCardFrontUrl.FileName)}";
                    string frontImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "AadharImages", frontImageName);

                    using (var stream = new FileStream(frontImagePath, FileMode.Create))
                    {
                        AadharCardFrontUrl.CopyTo(stream);
                    }
                    user.AadharCardFrontUrl = frontImageName;
                }

                // Process back image with different unique filename
                if (AadharCardBackUrl != null && AadharCardBackUrl.Length > 0)
                {
                    string backImageName = $"{Guid.NewGuid()}{Path.GetExtension(AadharCardBackUrl.FileName)}";
                    string backImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "AadharImages", backImageName);

                    using (var stream = new FileStream(backImagePath, FileMode.Create))
                    {
                        AadharCardBackUrl.CopyTo(stream);
                    }
                    user.AadharCardBackUrl = backImageName;
                }

                var response = _userMgmtService.CreateUserMaster(user);
                return Json(new
                {
                    success = response.statuCode == 1,
                    message = response.message,
                    redirectUrl = Url.Action("UserMaster") // Add redirect URL
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error: " + ex.Message
                });
            }
        }
        private string ProcessAadharImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            try
            {
                string fileName = GenerateUniqueFileName(imageFile.FileName);
                string uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "AadharImages");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string filePath = Path.Combine(uploadPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                return fileName; // Return just the filename
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            string sanitizedFileName = Path.GetFileNameWithoutExtension(originalFileName);
            sanitizedFileName = Regex.Replace(sanitizedFileName, @"[^\w]", ""); // Remove special characters
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string randomString = new Random().Next(1000, 9999).ToString();
            string extension = Path.GetExtension(originalFileName);

            return $"{timestamp}_{randomString}{extension}";
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserMasterUpdate(UserMaster User, IFormFile AadharCardFrontUrl, IFormFile AadharCardBackUrl)
        {
            try
            {
                var existingUser = _userMgmtService.UserMasterById(User.UserId);

                // Process front image update
                if (AadharCardFrontUrl != null && AadharCardFrontUrl.Length > 0)
                {
                    // Delete old front image if exists
                    if (!string.IsNullOrEmpty(existingUser.AadharCardFrontUrl))
                    {
                        string oldFrontPath = Path.Combine(_hostingEnvironment.WebRootPath, "AadharImages", existingUser.AadharCardFrontUrl);
                        if (System.IO.File.Exists(oldFrontPath))
                        {
                            System.IO.File.Delete(oldFrontPath);
                        }
                    }

                    string frontImageName = $"{Guid.NewGuid()}{Path.GetExtension(AadharCardFrontUrl.FileName)}";
                    string frontImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "AadharImages", frontImageName);

                    using (var stream = new FileStream(frontImagePath, FileMode.Create))
                    {
                        AadharCardFrontUrl.CopyTo(stream);
                    }
                    User.AadharCardFrontUrl = frontImageName;
                }
                else
                {
                    User.AadharCardFrontUrl = existingUser?.AadharCardFrontUrl;
                }

                // Process back image update
                if (AadharCardBackUrl != null && AadharCardBackUrl.Length > 0)
                {
                    // Delete old back image if exists
                    if (!string.IsNullOrEmpty(existingUser.AadharCardBackUrl))
                    {
                        string oldBackPath = Path.Combine(_hostingEnvironment.WebRootPath, "AadharImages", existingUser.AadharCardBackUrl);
                        if (System.IO.File.Exists(oldBackPath))
                        {
                            System.IO.File.Delete(oldBackPath);
                        }
                    }

                    string backImageName = $"{Guid.NewGuid()}{Path.GetExtension(AadharCardBackUrl.FileName)}";
                    string backImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "AadharImages", backImageName);

                    using (var stream = new FileStream(backImagePath, FileMode.Create))
                    {
                        AadharCardBackUrl.CopyTo(stream);
                    }
                    User.AadharCardBackUrl = backImageName;
                }
                else
                {
                    User.AadharCardBackUrl = existingUser?.AadharCardBackUrl;
                }

                var response = _userMgmtService.UpdateUserType(User);

                return Json(new
                {
                    success = response.statuCode == 1,
                    message = response.message,
                    redirectUrl = Url.Action("UserMaster") // Add redirect URL
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error updating user: " + ex.Message
                });
            }
        }


        [HttpPost]
        public IActionResult DeleteUserMaster([FromBody] UserMaster model)
        {

            if (model == null || model.UserId <= 0)
            {
                return Json(new { success = false, message = "Invalid ID" });
            }


            var res = _userMgmtService.DeleteUserType(model.UserId);

            if (res.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = res.message });
            }

        }

        // Helper method to process vehicle images
    
    }
}
