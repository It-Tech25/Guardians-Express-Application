using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class PurchaseDetailsRepo : IPurchaseDetailsRepo
    {
        private readonly MyDbContext _context;

        public PurchaseDetailsRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PurchaseDetailsEntity> GetPurchaselists()
        {
            var oo = _context.PurchaseDetails.Where(t=> t.IsDeleted == false).ToList();

            return oo;
        }


        // Get all purchase details (excluding deleted records)
        public IEnumerable<PurchaseDetailsDTO> GetPurchaseDetails()
        {
            var purchases = from pd in _context.PurchaseDetails
                            join b in _context.branch
                            on pd.BranchId equals b.id
                            where pd.IsDeleted == false  // Exclude deleted records
                            select new PurchaseDetailsDTO
                            {
                                PurchaseId = pd.PurchaseId,
                                Branch = b.BranchName,
                                BranchName = b.BranchName, // Joining Branch Name
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
                              
                                GrossAmount = pd.GrossAmount,
                                Discount = pd.Discount,
                                Tax = pd.Tax,
                                RoundOff = pd.RoundOff,
                                NetAmount = pd.NetAmount,
                                Notes = pd.Notes
                            };

            return purchases.ToList(); // Convert to list before returning
        }


        // Get a single purchase detail by ID
        public PurchaseDetailsDTO GetPurchaseById(int id)
        {
            var purchase = (from pd in _context.PurchaseDetails
                            join b in _context.branch
                            on pd.BranchId equals b.id
                            where pd.IsDeleted == false && pd.PurchaseId == id   
                            select new PurchaseDetailsDTO
                            {
                                PurchaseId = pd.PurchaseId,
                                Branch = b.BranchName,
                                BranchName = b.BranchName, // Joining Branch Name
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
                                
                                GrossAmount = pd.GrossAmount,
                                Discount = pd.Discount,
                                Tax = pd.Tax,
                                RoundOff = pd.RoundOff,
                                NetAmount = pd.NetAmount,
                                Notes = pd.Notes
                            }).FirstOrDefault();

            return purchase;
        }

        // Create a new purchase record
        public GenericResponse CreatePurchase(PurchaseDetailsDTO purchaseDto)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                PurchaseDetailsEntity purchase = new PurchaseDetailsEntity
                {
                    BranchId = purchaseDto.BranchId,
                    VoucherDate = purchaseDto.VoucherDate,
                    ClientName = purchaseDto.ClientName,
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
                 
                    GrossAmount = purchaseDto.GrossAmount,
                    Discount = purchaseDto.Discount,
                    Tax = purchaseDto.Tax,
                    RoundOff = purchaseDto.RoundOff,
                    NetAmount = purchaseDto.NetAmount,
                    Notes = purchaseDto.Notes,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now
                };

                _context.PurchaseDetails.Add(purchase);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Purchase created successfully";
                response.currentId = purchase.PurchaseId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to create purchase: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        // Update an existing purchase record
        public GenericResponse UpdatePurchase(PurchaseDetailsDTO purchaseDto)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var purchase = _context.PurchaseDetails.FirstOrDefault(p => p.PurchaseId == purchaseDto.PurchaseId);
                if (purchase != null)
                {
                    _context.Entry(purchase).CurrentValues.SetValues(purchaseDto);
                    purchase.UpdateOn = DateTime.Now;

                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Purchase updated successfully";
                    response.currentId = purchase.PurchaseId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Purchase not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update purchase: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        // Soft delete a purchase record
        public GenericResponse DeletePurchase(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var purchase = _context.PurchaseDetails.FirstOrDefault(p => p.PurchaseId == id);
                if (purchase != null)
                {
                    purchase.IsDeleted = true;
                    purchase.UpdateOn = DateTime.Now;
                    _context.Update(purchase);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Purchase deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete purchase: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
