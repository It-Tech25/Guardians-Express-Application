using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Controllers
{
    public class InvoiceReturnController : Controller
    {
        private readonly IInvoiceReturnService _service;
        private readonly IBranchMasterService _branchService;
        private readonly IAddressBookMasterService _addressService;
        private readonly MyDbContext _context;

        public InvoiceReturnController(IInvoiceReturnService service, IBranchMasterService branchService, IAddressBookMasterService addressService, MyDbContext context)
        {
            _service = service;
            _branchService = branchService;
            _addressService = addressService;
            _context = context;
        }

        // ----------------------------- List All Invoice Returns (Parent) -------------------------------------------
        public IActionResult InvoiceReturnIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
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
            var invoiceReturns = _service.GetInvoiceReturns();
            var totalRecords = invoiceReturns.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var invoiceReturnDtos = invoiceReturns.Select(ir => new InvoiceReturnModel
            {
                InvoiceReturnID = ir.InvoiceReturnID,
                BranchID = ir.BranchID,
                BranchName = ir.BranchName,
                InvoiceReturnDate = ir.InvoiceReturnDate,
                AccHead = ir.AccHead,
                InvoiceNo = ir.InvoiceNo,
                SalesType = ir.SalesType,
                InvoiceDate = ir.InvoiceDate,
                InvoiceAmount = ir.InvoiceAmount,
                Address = ir.Address,
                AccGSTIN = ir.AccGSTIN,
                CostCenter = ir.CostCenter,
                NoGST = ir.NoGST,
                DiscountOnMRP = ir.DiscountOnMRP,
                Notes = ir.Notes,
                GrossAmount = ir.GrossAmount,
                Discount = ir.Discount,
                Tax = ir.Tax,
                RoundOff = ir.RoundOff,
                NetAmount = ir.NetAmount,
                IsActive = ir.IsActive
            }).ToList();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;
            ViewBag.CostCenters = _context.branch.Where(a => a.IsDeleted == false).ToList();

           var branches = _branchService.BranchMaster(searchTerm, pageNumber, pageSize).ToList();
            ViewBag.Branches = branches;

            return View(invoiceReturnDtos);
        }

        // ----------------------------- Get Invoice Return by ID (Parent + Child) -------------------------------------------
        public IActionResult Details(int id)
        {
            var invoiceReturn = _service.GetInvoiceReturnById(id);
            if (invoiceReturn == null)
            {
                return NotFound();
            }

            // Get related invoice return items (child)
            var invoiceReturnDetails = _context.invoiceReturnDetails
                .Where(detail => detail.InvoiceReturnID == id)
                .ToList();

            ViewBag.InvoiceReturnDetails = invoiceReturnDetails;

            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            ViewBag.Branches = branches;

            return View(invoiceReturn);
        }

        // ----------------------------- Create Invoice Return (Parent) -------------------------------------------
        public IActionResult Create()
        {
            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            var addresses = _addressService.GetAddressBooks().ToList();

            ViewBag.Branches = branches;
            ViewBag.Addresses = addresses;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateInvoiceReturn(
            InvoiceReturnModel model,string serializedvoucherData)


        {
            var vdetails = JsonConvert.DeserializeObject<List<InvoiceReturnDetailsEntity>>(serializedvoucherData);

            if (model == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            try
            {
                // Create the main Invoice Return entity (Parent)
                var invoiceReturn = new InvoiceReturnEntity
                {
                    BranchID = model.BranchID,
                    InvoiceReturnDate = model.InvoiceReturnDate,
                    AccHead = model.AccHead,
                    InvoiceNo = model.InvoiceNo,
                    SalesType = model.SalesType,
                    InvoiceDate = model.InvoiceDate,
                    InvoiceAmount = model.InvoiceAmount,
                    Address = model.Address,
                    AccGSTIN = model.AccGSTIN,
                    CostCenter = model.CostCenter,
                    NoGST = model.NoGST,
                    DiscountOnMRP = model.DiscountOnMRP,
                    Notes = model.Notes,
                    GrossAmount = model.GrossAmount,
                    Discount = model.Discount,
                    Tax = model.Tax,
                    RoundOff = model.RoundOff,
                    NetAmount = model.NetAmount,
               
                    IsActive = true,
                    IsDeleted = false
                };

                // Call the service layer to add the Invoice Return (Parent)
                var response = _service.CreateInvoiceReturn(invoiceReturn);

                if (response.statuCode == 1)
                {
                    int invoiceReturnId = response.currentId; // Get the ID of the newly inserted Invoice Return

                    if (serializedvoucherData==null)
                    {
                        return Json(new { success = false, message = "Mismatch in input data. Ensure all lists have the same number of entries." });
                    }

                    // Save Invoice Return Items (Child)
                    List<InvoiceReturnDetailsEntity> invoiceReturnDetails = new List<InvoiceReturnDetailsEntity>();

                    foreach (var detail in vdetails)
                    {
                        detail.InvoiceReturnID = model.InvoiceReturnID;
                    }

                    _context.invoiceReturnDetails.AddRange(vdetails);
                    _context.SaveChanges();
                    return RedirectToAction("InvoiceReturnIndex");
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

        // ----------------------------- Delete Invoice Return (Parent + Child) -------------------------------------------
        [HttpPost]
        public IActionResult DeleteInvoiceReturn(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Invoice Return ID.");
            }

            try
            {
                // First, delete related invoice return items (child)
                var invoiceReturnDetails = _context.invoiceReturnDetails
                    .Where(item => item.InvoiceReturnID == id)
                    .ToList();

                _context.invoiceReturnDetails.RemoveRange(invoiceReturnDetails);
                _context.SaveChanges();

                // Then, delete the invoice return (parent)
                var response = _service.DeleteInvoiceReturn(id);

                if (response.statuCode == 1)
                {
                    return Json(new { success = true });
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
    }
}
