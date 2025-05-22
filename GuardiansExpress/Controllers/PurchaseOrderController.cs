using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuardiansExpress.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrderService _service;
        private readonly IBranchMasterService _branchService;
        private readonly MyDbContext _context;

        public PurchaseOrderController(IPurchaseOrderService service, IBranchMasterService branchService, MyDbContext context)
        {
            _service = service;
            _branchService = branchService;
            _context = context;
        }

        public IActionResult PurchaseOrderIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
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
            var orders = _service.GetPurchaseOrders(searchTerm, pageNumber, pageSize);
            var totalRecords = orders.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var purchaseOrderDtos = orders.Select(po => new PurchaseOrderModel
            {
                PurchaseId = po.PurchaseId,
                BranchId = po.BranchId,
                VoucherDate = po.VoucherDate,
                ClientName = po.ClientName,
                NoGST = po.NoGST,
                PaymentTerms = po.PaymentTerms,
                DeliveryTerms = po.DeliveryTerms,
                Packing = po.Packing,
                ShipTo = po.ShipTo,
                Transport = po.Transport,
                Insurance = po.Insurance,
                Freight = po.Freight,
                ValidFrom = po.ValidFrom,
                ValidTo = po.ValidTo,
                IndentNo = po.IndentNo,
                CostCenter = po.CostCenter,
                DiscountOnMRP = po.DiscountOnMRP,
                Notes = po.Notes,
                GrossAmount = po.GrossAmount,
                Discount = po.Discount,
                Tax = po.Tax,
                RoundOff = po.RoundOff,
                BranchName=po.BranchName,
                NetAmount = po.NetAmount,
                IsActive = po.IsActive
            }).ToList();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;

            var branches = _branchService.BranchMaster(searchTerm, pageNumber, pageSize).ToList();
            ViewBag.Branches = branches;

            return View(purchaseOrderDtos);
        }

        public IActionResult Details(int id)
        {
            var purchaseOrder = _service.GetPurchaseOrderById(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            // Get related purchase order items
            var orderItems = _context.purchaseOrderItems
                .Where(item => item.PurchaseId == id)
                .ToList();

            ViewBag.OrderItems = orderItems;

            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            ViewBag.Branches = branches;

            return View(purchaseOrder);
        }

        public IActionResult Create()
        {
            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            ViewBag.Branches = branches;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePurchaseOrder(
            PurchaseOrderModel model,
            List<string> Item,
            List<string> ItemDescription,
            List<string> HSNSAC,
            List<decimal> MRP,
            List<decimal> Rate,
            List<decimal> DiscountPercentage,
            List<int> Quantity,
            List<int> FreeQuantity,
            List<int> Stock,
            List<string> Unit,
            List<decimal> Amount,
            List<decimal> TaxPercentage,
            List<decimal> TaxAmount,
            List<decimal> TotalAmount)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }
            if (model.NoGST == null)
            {
                model.NoGST = false; 
            }

            try
            {
                // Create the main Purchase Order entity
                var purchaseOrder = new PurchaseOrderEntity
                {
                    BranchId = model.BranchId,
                    VoucherDate = model.VoucherDate ?? DateTime.Now,
                    ClientName = model.ClientName,
                    NoGST = model.NoGST,
                    PaymentTerms = model.PaymentTerms,
                    DeliveryTerms = model.DeliveryTerms,
                    Packing = model.Packing,
                    ShipTo = model.ShipTo,
                    Transport = model.Transport,
                    Insurance = model.Insurance,
                    Freight = model.Freight,
                    ValidFrom = model.ValidFrom,
                    ValidTo = model.ValidTo,
                    IndentNo = model.IndentNo,
                    CostCenter = model.CostCenter,
                    DiscountOnMRP = model.DiscountOnMRP,
                    Notes = model.Notes,
                    GrossAmount = model.GrossAmount,
                    Discount = model.Discount,
                    Tax = model.Tax,
                    RoundOff = model.RoundOff,
                    NetAmount = model.NetAmount,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                };

                // Call the service layer to add the purchase order
                var response = _service.CreatePurchaseOrder(purchaseOrder);

                if (response.statuCode == 1)
                {
                    int purchaseId = response.currentId; // Get the ID of the newly inserted record

                    // Validate that all input lists have the same length
                    int itemCount = Item.Count;
                    if (ItemDescription.Count != itemCount ||
                        HSNSAC.Count != itemCount ||
                        MRP.Count != itemCount ||
                        Rate.Count != itemCount ||
                        DiscountPercentage.Count != itemCount ||
                        Quantity.Count != itemCount ||
                        FreeQuantity.Count != itemCount ||
                        Stock.Count != itemCount ||
                        Unit.Count != itemCount ||
                        Amount.Count != itemCount ||
                        TaxPercentage.Count != itemCount ||
                        TaxAmount.Count != itemCount ||
                        TotalAmount.Count != itemCount)
                    {
                        return Json(new { success = false, message = "Mismatch in input data. Ensure all lists have the same number of entries." });
                    }

                    // Save Purchase Order Items
                    List<PurchaseOrderItemEntity> orderItems = new List<PurchaseOrderItemEntity>();

                    for (int i = 0; i < itemCount; i++)
                    {
                        var orderItem = new PurchaseOrderItemEntity
                        {
                            PurchaseId = purchaseId,
                            Item = Item[i],
                            ItemDescription = ItemDescription[i],
                            HSNSAC = HSNSAC[i],
                            MRP = MRP[i],
                            Rate = Rate[i],
                            DiscountPercentage = DiscountPercentage[i],
                            Quantity = Quantity[i],
                            FreeQuantity = FreeQuantity[i],
                            Stock = Stock[i],
                            Unit = Unit[i],
                            Amount = Amount[i],
                            TaxPercentage = TaxPercentage[i],
                            TaxAmount = TaxAmount[i],
                            TotalAmount = TotalAmount[i]
                        };

                        orderItems.Add(orderItem);
                    }

                    // Add purchase order items to the database
                    _context.purchaseOrderItems.AddRange(orderItems);
                    _context.SaveChanges();

                    return RedirectToAction("PurchaseOrderIndex");
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

        public IActionResult GetPurchaseOrderById(int id)
        {
            var purchaseOrder = _service.GetPurchaseOrderById(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            // Get related purchase order items
            var orderItems = _context.purchaseOrderItems
                .Where(item => item.PurchaseId == id)
                .ToList();

            ViewBag.OrderItems = orderItems;

            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            ViewBag.Branches = branches;

            return View(purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PurchaseOrderEdit(
            PurchaseOrderEntity model,
            List<int> ItemId,
            List<string> Item,
            List<string> ItemDescription,
            List<string> HSNSAC,
            List<decimal> MRP,
            List<decimal> Rate,
            List<decimal> DiscountPercentage,
            List<int> Quantity,
            List<int> FreeQuantity,
            List<int> Stock,
            List<string> Unit,
            List<decimal> Amount,
            List<decimal> TaxPercentage,
            List<decimal> TaxAmount,
            List<decimal> TotalAmount)
        {
            try
            {
                // Update purchase order
                model.UpdatedOn = DateTime.Now;
                var response = _service.UpdatePurchaseOrder(model);

                if (response.statuCode == 1)
                {
                    // Remove existing order items
                    var existingItems = _context.purchaseOrderItems
                        .Where(item => item.PurchaseId == model.PurchaseId)
                        .ToList();

                    _context.purchaseOrderItems.RemoveRange(existingItems);
                    _context.SaveChanges();

                    // Add updated order items
                    List<PurchaseOrderItemEntity> orderItems = new List<PurchaseOrderItemEntity>();

                    for (int i = 0; i < Item.Count; i++)
                    {
                        var orderItem = new PurchaseOrderItemEntity
                        {
                            ItemId = i < ItemId.Count ? ItemId[i] : 0, // Use existing ID if available
                            PurchaseId = model.PurchaseId,
                            Item = Item[i],
                            ItemDescription = ItemDescription[i],
                            HSNSAC = HSNSAC[i],
                            MRP = MRP[i],
                            Rate = Rate[i],
                            DiscountPercentage = DiscountPercentage[i],
                            Quantity = Quantity[i],
                            FreeQuantity = FreeQuantity[i],
                            Stock = Stock[i],
                            Unit = Unit[i],
                            Amount = Amount[i],
                            TaxPercentage = TaxPercentage[i],
                            TaxAmount = TaxAmount[i],
                            TotalAmount = TotalAmount[i]
                        };

                        orderItems.Add(orderItem);
                    }

                    _context.purchaseOrderItems.AddRange(orderItems);
                    _context.SaveChanges();

                    return RedirectToAction("PurchaseOrderIndex");
                }

                return Json(new { success = false, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeletePurchaseOrder(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Purchase Order ID.");
            }

            try
            {
                // First, delete related order items
                var orderItems = _context.purchaseOrderItems
                    .Where(item => item.PurchaseId == id)
                    .ToList();

                _context.purchaseOrderItems.RemoveRange(orderItems);
                _context.SaveChanges();

                // Then, delete the purchase order
                var response = _service.DeletePurchaseOrder(id);

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