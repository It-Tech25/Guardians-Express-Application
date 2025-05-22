using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
    public class PurchaseOrderRepo : IPurchaseOrderRepo
    {
        private readonly MyDbContext _context;

        public PurchaseOrderRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PurchaseOrderModel> GetPurchaseOrders(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from po in _context.purchaseOrders
                        join branch in _context.branch on po.BranchId equals branch.id
                        where po.IsDeleted==false
                        select new PurchaseOrderModel
                        {
                            PurchaseId = po.PurchaseId,
                            ClientName = po.ClientName, // Assuming you want to use ClientName instead of SupplierName
                            VoucherDate = po.VoucherDate, // Mapping the appropriate property
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
                            NetAmount = po.NetAmount,
                            CreatedOn = po.CreatedOn,
                            UpdatedOn = po.UpdatedOn,
                            IsActive = po.IsActive,
                            IsDeleted = po.IsDeleted,
                            BranchId = po.BranchId,
                            BranchName = branch.BranchName
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(po => po.ClientName.Contains(searchTerm) ||
                                          po.IndentNo.Contains(searchTerm)); // You can change fields based on the search requirement
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }



        public PurchaseOrderEntity GetPurchaseOrderById(int id)
        {
            return _context.purchaseOrders.Find(id);
        }

        public GenericResponse CreatePurchaseOrder(PurchaseOrderEntity purchaseOrder)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                purchaseOrder.IsDeleted = false;
                _context.purchaseOrders.Add(purchaseOrder);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Purchase Order created successfully";
                response.currentId = purchaseOrder.PurchaseId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Purchase Order: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse UpdatePurchaseOrder(PurchaseOrderEntity purchaseOrder)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.purchaseOrders.FirstOrDefault(po => po.PurchaseId == purchaseOrder.PurchaseId);
                if (existing != null)
                {
                    purchaseOrder.UpdatedOn = DateTime.Now;
                    _context.Entry(existing).CurrentValues.SetValues(purchaseOrder);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Purchase Order updated successfully";
                    response.currentId = purchaseOrder.PurchaseId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Purchase Order not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Purchase Order: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeletePurchaseOrder(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.purchaseOrders.FirstOrDefault(po => po.PurchaseId == id);
                if (existing != null)
                {
                    existing.IsDeleted = true;
                    _context.Update(existing);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Purchase Order deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Purchase Order: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
