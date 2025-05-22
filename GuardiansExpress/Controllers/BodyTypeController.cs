using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class BodyTypeController : Controller
    {
        private readonly IBodyTypeService _bodyTypeService;
        private readonly MyDbContext _context;

        public BodyTypeController(IBodyTypeService bodyTypeService, MyDbContext context)
        {
            _bodyTypeService = bodyTypeService;
            _context = context;
        }

        public IActionResult BodyTypeIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            LoginResponse lr = new LoginResponse();
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;

            var bodyTypesQuery = _bodyTypeService.GetBodyTypes().AsQueryable();

            // Apply search filter if search term exists
            if (!string.IsNullOrEmpty(searchTerm))
            {
                bodyTypesQuery = bodyTypesQuery.Where(bt =>
                    bt.Bodytype.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            int totalRecords = bodyTypesQuery.Count();
            var bodyTypes = bodyTypesQuery.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            ViewData["SearchTerm"] = searchTerm;
            ViewData["PageSize"] = pageSize;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(bodyTypes);
        }

        public IActionResult BodyTypeAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BodyTypeAdd(BodyTypeEntity bodyTypeModel)
        {
            // Normalize the input for comparison
            bodyTypeModel.Bodytype = bodyTypeModel.Bodytype?.Trim();

            // Check if body type is empty
            if (string.IsNullOrWhiteSpace(bodyTypeModel.Bodytype))
            {
                return Json(new { success = false, message = "Body type cannot be empty." });
            }

            // Check for duplicates (case-insensitive)
            var exists = _bodyTypeService.BodyTypeExists(bodyTypeModel.Bodytype);
            if (exists)
            {
                return Json(new { success = false, message = "This body type already exists." });
            }

            var response = _bodyTypeService.CreateBodyType(bodyTypeModel);
            return Json(new
            {
                success = response.statuCode == 1,
                message = response.message
            });
        }

        public IActionResult BodyTypeEdit(int id)
        {
            var bodyType = _bodyTypeService.GetBodyTypeById(id);
            if (bodyType == null)
            {
                return NotFound();
            }
            return View(bodyType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BodyTypeUpdate(BodyTypeEntity model)
        {
            // Normalize the input for comparison
            model.Bodytype = model.Bodytype?.Trim();

            // Check if body type is empty
            if (string.IsNullOrWhiteSpace(model.Bodytype))
            {
                return Json(new { success = false, message = "Body type cannot be empty." });
            }

            // Check if another body type with the same name exists (excluding current one)
            var existing = _bodyTypeService.GetBodyTypeByName(model.Bodytype);
            if (existing != null && existing.Id != model.Id)
            {
                return Json(new { success = false, message = "Another body type with this name already exists." });
            }

            var response = _bodyTypeService.UpdateBodyType(model);
            return Json(new
            {
                success = response.statuCode == 1,
                message = response.message
            });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Body Type ID.");
            }

            var response = _bodyTypeService.DeleteBodyType(id);
            return Json(new
            {
                success = response.statuCode == 1,
                message = response.message
            });
        }
    }
}