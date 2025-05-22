using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace GuardiansExpress.Controllers
{
    public class TaxCategoryController : Controller
    {
        private readonly ITaxCategoryService _service;
        private readonly MyDbContext _context;
        private readonly ITaxMasterService _tService;

        public TaxCategoryController(ITaxCategoryService service, MyDbContext context, ITaxMasterService tService)
        {
           _service = service;
            _context = context;
            _tService= tService;
        }

        public IActionResult TaxCategory(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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

            var query = _context.taxCategory
                       .Where(a => a.IsDeleted == false &&
                                  (string.IsNullOrEmpty(searchTerm) || a.CategoryName.Contains(searchTerm)));

           
            int totalRecords = query.Count();

            
            var taxEntities = query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

            
            var taxDTOs = taxEntities.Select(entity => new TaxCategoryDTO
            {
                ID = entity.ID,
                CategoryName = entity.CategoryName,
                Status = entity.Status,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                CreatedOn = entity.CreatedOn,
                CreatedBy = entity.CreatedBy,
                UpdatedOn = entity.UpdatedOn,
                UpdatedBy = entity.UpdatedBy,
                
            }).ToList();

           
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

         
            var s = _tService.GetTaxMastersWithLedger(searchTerm, pageNumber, pageSize).ToList();
            ViewBag.TaxMasters = s;
           

           
            return View(taxDTOs);
        }

        [HttpGet]
        public IActionResult GetTaxCategory(int id)
        {
            var taxCategory = _context.taxCategory.Find(id);
            if (taxCategory == null)
            {
                return NotFound();
            }

            var taxDetails = _context.TaxDetailsTableEntitys
                .Where(td => td.id == id)
                .ToList();

            var dto = new TaxCategoryDTO
            {
                ID = taxCategory.ID,
                CategoryName = taxCategory.CategoryName,
                Status = taxCategory.Status,
                IsActive = taxCategory.IsActive,
                
            };

            var response = new
            {
                id = dto.ID,
                categoryName = dto.CategoryName,
                isActive = dto.IsActive,
                taxDetails = taxDetails.Select(td => new
                {
                    taxMasterID = td.TaxMasterID,
                    value = td.Value,
                    taxFor = td.TaxFor,
                    isActive = td.IsActive
                }).ToList()
            };

            return Json(response);
        }  

        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public IActionResult UpdateTaxCategory(TaxCategoryDTO model, List<int> TaxMasterID, List<decimal> Value, List<string> TaxFor)
        {
            if (model == null || TaxMasterID == null || Value == null || TaxFor == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            try
            {
                
                var existingTaxCategory = _context.taxCategory.Find(model.ID);

                if (existingTaxCategory == null)
                {
                    return Json(new { success = false, message = "Tax Category not found." });
                }

                existingTaxCategory.CategoryName = model.CategoryName;
                existingTaxCategory.Status = model.Status;
                existingTaxCategory.IsActive = true;
                existingTaxCategory.IsDeleted = false;

                
                _context.SaveChanges();

                
                var existingTaxDetails = _context.TaxDetailsTableEntitys.Where(td => td.id == model.ID).ToList();
                if (existingTaxDetails.Any())
                {
                    _context.TaxDetailsTableEntitys.RemoveRange(existingTaxDetails);
                }

                
                List<TaxDetailsTableEntity> newTaxDetails = new List<TaxDetailsTableEntity>();

                for (int i = 0; i < TaxMasterID.Count; i++)
                {
                    var newTaxDetail = new TaxDetailsTableEntity
                    {
                        id = model.ID,
                        TaxMasterID = TaxMasterID[i],
                        Value = Value[i],
                        TaxFor = TaxFor[i],
                        IsActive = true,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedBy = "Admin" 
                    };

                    newTaxDetails.Add(newTaxDetail);
                }

                _context.TaxDetailsTableEntitys.AddRange(newTaxDetails);
                _context.SaveChanges();

                return RedirectToAction("TaxCategory");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


        [HttpPost]
        public IActionResult CreateTax(
     TaxCategoryEntity model,
     List<int> TaxMasterID,
     List<decimal> Value,
     List<string> TaxFor)
        {
            if (model == null || TaxMasterID == null || Value == null || TaxFor == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            try
            {
                
                var newTaxCategory = new TaxCategoryEntity
                {
                    ID = model.ID,
                    CategoryName = model.CategoryName,
                    Status = model.Status
                };

                var response = _service.CreateTaxCategory(newTaxCategory, TaxMasterID, Value, TaxFor);

                if (response.statuCode == 1)
                {


                    return RedirectToAction("TaxCategory");

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

        [HttpPost]
        public IActionResult DeleteTaxCategory(int id)
        {
            try
            {
                
                var taxDetails = _context.TaxDetailsTableEntitys.Where(td => td.id == id).ToList();

                if (taxDetails.Any())
                {
                    
                    _context.TaxDetailsTableEntitys.RemoveRange(taxDetails);
                    _context.SaveChanges(); // Save immediately to avoid foreign key conflicts
                }

                
                var taxCategory = _context.taxCategory.Find(id);
                if (taxCategory == null)
                {
                    return Json(new { success = false, message = "Tax Category not found." });
                }

                
                _context.taxCategory.Remove(taxCategory);
                _context.SaveChanges();

                return Json(new { success = true, message = "Tax Category deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

    }
}
