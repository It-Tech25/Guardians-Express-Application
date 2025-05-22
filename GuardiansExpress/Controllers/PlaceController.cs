using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GuardiansExpress.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IPlaceRepo _placeRepo;
        private readonly MyDbContext _context;

        public PlaceController(IPlaceRepo placeRepo, MyDbContext context)
        {
            _placeRepo = placeRepo;
            _context = context;
        }

        //----------------------------- Get All Places -------------------------------------------
        public IActionResult PlaceIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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

            var places = _placeRepo.GetPlaces(searchTerm, pageNumber, pageSize);
            var totalRecords = _context.placeEntity.Count(p => p.IsDeleted == false &&
                                                             (string.IsNullOrEmpty(searchTerm) ||
                                                              p.PlaceName.Contains(searchTerm)));

            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(places);
        }

        //----------------------------- Add Place -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceAdd(Place place)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data provided." });
            }

            // Check for duplicate place name
            bool placeExists = _context.placeEntity
                .Any(p => p.PlaceName.ToLower() == place.PlaceName.ToLower() && p.IsDeleted == false);

            if (placeExists)
            {
                return Json(new { success = false, message = "A place with this name already exists." });
            }

            var response = _placeRepo.CreatePlace(place);
            return Json(new
            {
                success = response.statuCode == 1,
                message = response.message
            });
        }

        //----------------------------- Edit Place (GET) -------------------------------------------
        public IActionResult PlaceEdit(int id)
        {
            var place = _placeRepo.GetPlaceById(id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        //----------------------------- Edit Place (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceUpdate(Place model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data provided." });
            }

            // Check for duplicate place name (excluding current record)
            bool placeExists = _context.placeEntity
                .Any(p => p.PlaceName.ToLower() == model.PlaceName.ToLower()
                        && p.Id != model.Id
                        && p.IsDeleted == false);

            if (placeExists)
            {
                return Json(new { success = false, message = "A place with this name already exists." });
            }

            var response = _placeRepo.UpdatePlace(model);
            return Json(new
            {
                success = response.statuCode == 1,
                message = response.message
            });
        }

        //----------------------------- Delete Place -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "Invalid Place ID." });
            }

            try
            {
                var response = _placeRepo.DeletePlace(id);

                if (response == null)
                {
                    return Json(new { success = false, message = "Failed to delete place. Response was null." });
                }

                return Json(new
                {
                    success = response.statuCode == 1,
                    message = response.message
                });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new
                {
                    success = false,
                    message = $"An error occurred while deleting the place: {ex.Message}"
                });
            }
        }

        //----------------------------- Check for Duplicate Place Name (for AJAX validation) -------------------------------------------
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPlaceName(string placeName, int id = 0)
        {
            bool exists = _context.placeEntity
                .Any(p => p.PlaceName.ToLower() == placeName.ToLower()
                        && p.Id != id
                        && p.IsDeleted == false);

            return Json(!exists);
        }
    }
}