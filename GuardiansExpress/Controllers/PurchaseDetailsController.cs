using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class PurchaseDetailsController : Controller
    {
        private readonly IPurchaseDetailsService _purchaseDetailsService;
        private readonly IBranchMasterService _branchMasterService;

        public PurchaseDetailsController(IPurchaseDetailsService purchaseDetailsService, IBranchMasterService branchMasterService)
        {
            _purchaseDetailsService = purchaseDetailsService;
            _branchMasterService = branchMasterService;
        }


        // ----------------------------- List All Purchase Details (Index) -------------------------------------------
        public IActionResult Index(string searchTerm, int pageNumber, int pageSize)
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
            try
            {
                var purchaseDetails = _purchaseDetailsService.GetPurchaseDetails(searchTerm, pageNumber, pageSize);
                var purchaseDetailsDtos = purchaseDetails.Select(entity => new PurchaseDetailsDTO
                {
                    PurchaseId = entity.PurchaseId,
                    Branch = entity.Branch,
                    VoucherDate = entity.VoucherDate,
                    ClientName = entity.ClientName,
                    NoGST = entity.NoGST,
                    PaymentTerms = entity.PaymentTerms,
                    DeliveryTerms = entity.DeliveryTerms,
                    Packing = entity.Packing,
                    ShipTo = entity.ShipTo,
                    Transport = entity.Transport,
                    Insurance = entity.Insurance,
                    Freight = entity.Freight,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    IndentNo = entity.IndentNo,
                    CostCenter = entity.CostCenter,
                    DiscountOnMRP = entity.DiscountOnMRP,
                    ItemName = entity.ItemName,
                    Description = entity.Description,
                    HSN_SAC = entity.HSN_SAC,
                    MRP = entity.MRP,
                    Rate = entity.Rate,
                    DiscountPercentage = entity.DiscountPercentage,
                    Quantity = entity.Quantity,
                    FreeQuantity = entity.FreeQuantity,
                    Stock = entity.Stock,
                    Unit = entity.Unit,
                    Amount = entity.Amount,
                    TaxPercentage = entity.TaxPercentage,
                    TaxAmount = entity.TaxAmount,
                    TotalAmount = entity.TotalAmount,
                    GrossAmount = entity.GrossAmount,
                    Discount = entity.Discount,
                    Tax = entity.Tax,
                    RoundOff = entity.RoundOff,
                    NetAmount = entity.NetAmount,
                    Notes = entity.Notes,
                    IsActive = entity.IsActive,
                    IsDeleted = entity.IsDeleted,
                    CreatedOn = entity.CreatedOn,
                    CreatedBy = entity.CreatedBy,
                    UpdateOn = entity.UpdateOn,
                    UpdatedBy = entity.UpdatedBy
                }).ToList();

                var oo = _purchaseDetailsService.GetPurchaseDetails(searchTerm, pageNumber, pageSize).ToList();
                var ww = _purchaseDetailsService.GetPurchaselists().ToList();
                 
                var s = _branchMasterService.BranchMaster(searchTerm, pageNumber, pageSize).ToList();
                ViewBag.s = s;

                return View(purchaseDetailsDtos);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving purchase details.";
                return View("Error");
            }
        }

        // ----------------------------- Get Purchase Detail by ID -------------------------------------------
        public IActionResult Details(int id)
        {
            var entity = _purchaseDetailsService.GetPurchaseDetailById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var purchaseDto = new PurchaseDetailsDTO
            {
                PurchaseId = entity.PurchaseId,
                ClientName = entity.ClientName,
                Branch = entity.Branch,
                VoucherDate = entity.VoucherDate,
                NoGST = entity.NoGST,
                PaymentTerms = entity.PaymentTerms,
                DeliveryTerms = entity.DeliveryTerms,
                Packing = entity.Packing,
                ShipTo = entity.ShipTo,
                Transport = entity.Transport,
                Insurance = entity.Insurance,
                Freight = entity.Freight,
                ValidFrom = entity.ValidFrom,
                ValidTo = entity.ValidTo,
                IndentNo = entity.IndentNo,
                CostCenter = entity.CostCenter,
                DiscountOnMRP = entity.DiscountOnMRP,
                ItemName = entity.ItemName,
                Description = entity.Description,
                HSN_SAC = entity.HSN_SAC,
                MRP = entity.MRP,
                Rate = entity.Rate,
                DiscountPercentage = entity.DiscountPercentage,
                Quantity = entity.Quantity,
                FreeQuantity = entity.FreeQuantity,
                Stock = entity.Stock,
                Unit = entity.Unit,
                Amount = entity.Amount,
                TaxPercentage = entity.TaxPercentage,
                TaxAmount = entity.TaxAmount,
                TotalAmount = entity.TotalAmount,
                GrossAmount = entity.GrossAmount,
                Discount = entity.Discount,
                Tax = entity.Tax,
                RoundOff = entity.RoundOff,
                NetAmount = entity.NetAmount,
                Notes = entity.Notes,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                CreatedOn = entity.CreatedOn,
                CreatedBy = entity.CreatedBy,
                UpdateOn = entity.UpdateOn,
                UpdatedBy = entity.UpdatedBy
            };

            return View(purchaseDto);
        }

        // ----------------------------- Add Purchase (GET) -------------------------------------------
        public IActionResult Create()
        {
            return View();
        }

        // ----------------------------- Add Purchase (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult  Create(PurchaseDetailsDTO purchaseDto)
        {
            if (ModelState.IsValid)
            {
                var entity = new PurchaseDetailsDTO
                {
                    PurchaseId = purchaseDto.PurchaseId,
                    ClientName = purchaseDto.ClientName,
                    Branch = purchaseDto.Branch,
                    VoucherDate = purchaseDto.VoucherDate,
                    NoGST = purchaseDto.NoGST,
                    PaymentTerms = purchaseDto.PaymentTerms,
                    DeliveryTerms = purchaseDto.DeliveryTerms,
                    Packing = purchaseDto.Packing,
                    ShipTo = purchaseDto.ShipTo,
                    Transport = purchaseDto.Transport,
                    Insurance = purchaseDto.Insurance,
                    Freight = purchaseDto.Freight,
                    ValidFrom = purchaseDto.ValidFrom,
                    ValidTo = purchaseDto.ValidTo,
                    IndentNo = purchaseDto.IndentNo,
                    CostCenter = purchaseDto.CostCenter,
                    DiscountOnMRP = purchaseDto.DiscountOnMRP,
                    ItemName = purchaseDto.ItemName,
                    Description = purchaseDto.Description,
                    HSN_SAC = purchaseDto.HSN_SAC,
                    MRP = purchaseDto.MRP,
                    Rate = purchaseDto.Rate,
                    DiscountPercentage = purchaseDto.DiscountPercentage,
                    Quantity = purchaseDto.Quantity,
                    FreeQuantity = purchaseDto.FreeQuantity,
                    Stock = purchaseDto.Stock,
                    Unit = purchaseDto.Unit,
                    Amount = purchaseDto.Amount,
                    TaxPercentage = purchaseDto.TaxPercentage,
                    TaxAmount = purchaseDto.TaxAmount,
                    TotalAmount = purchaseDto.TotalAmount,
                    GrossAmount = purchaseDto.GrossAmount,
                    Discount = purchaseDto.Discount,
                    Tax = purchaseDto.Tax,
                    RoundOff = purchaseDto.RoundOff,
                    NetAmount = purchaseDto.NetAmount,
                    Notes = purchaseDto.Notes,
                    IsActive = purchaseDto.IsActive,
                    IsDeleted = purchaseDto.IsDeleted,
                    CreatedOn = DateTime.Now
                };

                var response = _purchaseDetailsService.CreatePurchaseDetail(entity);
                if (response.statuCode == 1)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", response.message);
            }
            return View(purchaseDto);
        }

        // ----------------------------- Delete Purchase -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = _purchaseDetailsService.DeletePurchaseDetail(id);
            if (response.statuCode == 1)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", response.message);
            return RedirectToAction("Index");
        }
    }
}
