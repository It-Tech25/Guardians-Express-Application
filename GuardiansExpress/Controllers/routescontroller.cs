using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class routescontroller : Controller
    {
        private readonly IRoutesService _routesService;
        private readonly MyDbContext _context;

        // Constructor injection for Routes Service
        public routescontroller(IRoutesService routesService, MyDbContext context)
        {
            _routesService = routesService;
            _context = context;
        }

        //----------------------------- Get All Routes -------------------------------------------
        public IActionResult RoutesIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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
            var query = _context.routes
                        .Where(r => r.IsDelete == false &&
                                   (string.IsNullOrEmpty(searchTerm) || r.from_place.Contains(searchTerm) || r.to_place.Contains(searchTerm)));

            // Get total count after filtering
            int totalRecords = query.Count();

            // Apply pagination
            var routes = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Set view data
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(routes);
        }

        //----------------------------- Add Route -------------------------------------------
        [HttpPost]
        public IActionResult RouteAdd(Routes route)
        {
            // Call service to create a new route
            var response = _routesService.CreateRoute(route);
            if (response.statuCode == 1)
            {
                return RedirectToAction("RoutesIndex");
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        //----------------------------- Edit Route (GET) -------------------------------------------
        public IActionResult RouteEdit(int id)
        {
            // Retrieve the Route by its ID for editing
            var route = _routesService.GetRouteById(id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        //----------------------------- Edit Route (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RouteUpdate(Routes model)
        {
            var response = _routesService.UpdateRoute(model);
            if (response.statuCode == 1)
            {
                return RedirectToAction("RoutesIndex");
            }
            return Json(new { success = false, message = response.message });
        }

        //----------------------------- Delete Route -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return BadRequest("Invalid Route ID.");
            }

            // Call service to delete the route
            var response = _routesService.DeleteRoute(id);

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
