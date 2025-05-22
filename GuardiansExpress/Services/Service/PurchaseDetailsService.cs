using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Services
{
    public class PurchaseDetailsService : IPurchaseDetailsService
    {
        private readonly IPurchaseDetailsRepo _purchaseDetailsRepo;

        public PurchaseDetailsService(IPurchaseDetailsRepo purchaseDetailsRepo)
        {
            _purchaseDetailsRepo = purchaseDetailsRepo;
        }


        public IEnumerable<PurchaseDetailsEntity> GetPurchaselists()
        {
           return _purchaseDetailsRepo.GetPurchaselists();
        }

        // Get all purchase details records as DTOs
        public IEnumerable<PurchaseDetailsDTO> GetPurchaseDetails(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                // Retrieve purchase details from the repository
                var purchaseDetails = _purchaseDetailsRepo.GetPurchaseDetails();

                // Map entities to DTOs before returning
                var purchaseDetailsDtos = purchaseDetails.Select(pd => new PurchaseDetailsDTO
                {
                    PurchaseId = pd.PurchaseId,
                    Branch = pd.Branch,
                    VoucherDate = pd.VoucherDate,
                    ClientName = pd.ClientName,
                    NoGST = pd.NoGST,
                    PaymentTerms = pd.PaymentTerms,
                    DeliveryTerms = pd.DeliveryTerms,
                    Packing = pd.Packing,
                    ShipTo = pd.ShipTo,
                    Transport = pd.Transport,
                    Insurance = pd.Insurance,
                    Freight = pd.Freight,
                    ValidFrom = pd.ValidFrom,
                    ValidTo = pd.ValidTo,
                    IndentNo = pd.IndentNo,
                    CostCenter = pd.CostCenter,
                    DiscountOnMRP = pd.DiscountOnMRP,
                    ItemName = pd.ItemName,
                    Description = pd.Description,
                    HSN_SAC = pd.HSN_SAC,
                    MRP = pd.MRP,
                    Rate = pd.Rate,
                    DiscountPercentage = pd.DiscountPercentage,
                    Quantity = pd.Quantity,
                    FreeQuantity = pd.FreeQuantity,
                    Stock = pd.Stock,
                    Unit = pd.Unit,
                    Amount = pd.Amount,
                    TaxPercentage = pd.TaxPercentage,
                    TaxAmount = pd.TaxAmount,
                    TotalAmount = pd.TotalAmount,
                    GrossAmount = pd.GrossAmount,
                    Discount = pd.Discount,
                    Tax = pd.Tax,
                    RoundOff = pd.RoundOff,
                    NetAmount = pd.NetAmount,
                    Notes = pd.Notes,
                    IsActive = pd.IsActive,
                    IsDeleted = pd.IsDeleted,
                    CreatedOn = pd.CreatedOn,
                    CreatedBy = pd.CreatedBy,
                    UpdateOn = pd.UpdateOn,
                    UpdatedBy = pd.UpdatedBy
                }).ToList();

                return purchaseDetailsDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving purchase details.", ex);
            }
        }

        // Get a single purchase detail by its ID as DTO
        public PurchaseDetailsDTO GetPurchaseDetailById(int id)
        {
            try
            {
                // Retrieve a single purchase detail from the repository
                var purchaseDetail = _purchaseDetailsRepo.GetPurchaseById(id);

                // Map entity to DTO before returning
                if (purchaseDetail == null) return null;

                return new PurchaseDetailsDTO
                {
                    PurchaseId = purchaseDetail.PurchaseId,
                    Branch = purchaseDetail.Branch,
                    VoucherDate = purchaseDetail.VoucherDate,
                    ClientName = purchaseDetail.ClientName,
                    NoGST = purchaseDetail.NoGST,
                    PaymentTerms = purchaseDetail.PaymentTerms,
                    DeliveryTerms = purchaseDetail.DeliveryTerms,
                    Packing = purchaseDetail.Packing,
                    ShipTo = purchaseDetail.ShipTo,
                    Transport = purchaseDetail.Transport,
                    Insurance = purchaseDetail.Insurance,
                    Freight = purchaseDetail.Freight,
                    ValidFrom = purchaseDetail.ValidFrom,
                    ValidTo = purchaseDetail.ValidTo,
                    IndentNo = purchaseDetail.IndentNo,
                    CostCenter = purchaseDetail.CostCenter,
                    DiscountOnMRP = purchaseDetail.DiscountOnMRP,
                    ItemName = purchaseDetail.ItemName,
                    Description = purchaseDetail.Description,
                    HSN_SAC = purchaseDetail.HSN_SAC,
                    MRP = purchaseDetail.MRP,
                    Rate = purchaseDetail.Rate,
                    DiscountPercentage = purchaseDetail.DiscountPercentage,
                    Quantity = purchaseDetail.Quantity,
                    FreeQuantity = purchaseDetail.FreeQuantity,
                    Stock = purchaseDetail.Stock,
                    Unit = purchaseDetail.Unit,
                    Amount = purchaseDetail.Amount,
                    TaxPercentage = purchaseDetail.TaxPercentage,
                    TaxAmount = purchaseDetail.TaxAmount,
                    TotalAmount = purchaseDetail.TotalAmount,
                    GrossAmount = purchaseDetail.GrossAmount,
                    Discount = purchaseDetail.Discount,
                    Tax = purchaseDetail.Tax,
                    RoundOff = purchaseDetail.RoundOff,
                    NetAmount = purchaseDetail.NetAmount,
                    Notes = purchaseDetail.Notes,
                    IsActive = purchaseDetail.IsActive,
                    IsDeleted = purchaseDetail.IsDeleted,
                    CreatedOn = purchaseDetail.CreatedOn,
                    CreatedBy = purchaseDetail.CreatedBy,
                    UpdateOn = purchaseDetail.UpdateOn,
                    UpdatedBy = purchaseDetail.UpdatedBy
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the purchase detail by ID.", ex);
            }
        }

        // Create a new purchase detail from a DTO
        public GenericResponse CreatePurchaseDetail(PurchaseDetailsDTO purchaseDetailDto)
        {
            try
            {
                
                // Create the purchase detail using the repository
                return _purchaseDetailsRepo.CreatePurchase(purchaseDetailDto);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the purchase detail.",
                    currentId = 0
                };
            }
        }

        // Update an existing purchase detail using a DTO
        public GenericResponse UpdatePurchaseDetail(PurchaseDetailsDTO purchaseDetailDto)
        {
            try
            {
             
                // Update the purchase detail using the repository
                return _purchaseDetailsRepo.UpdatePurchase(purchaseDetailDto);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the purchase detail.",
                    currentId = 0
                };
            }
        }

        // Delete a purchase detail by ID
        public GenericResponse DeletePurchaseDetail(int id)
        {
            try
            {
                // Delete the purchase detail using the repository
                return _purchaseDetailsRepo.DeletePurchase(id);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the purchase detail.",
                    currentId = 0
                };
            }
        }
    }
}
