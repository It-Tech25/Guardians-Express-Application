using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepo _purchaseOrderRepo;

        public PurchaseOrderService(IPurchaseOrderRepo purchaseOrderRepo)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
        }

        public IEnumerable<PurchaseOrderModel> GetPurchaseOrders(string searchTerm, int pageNumber, int pageSize)
        {
            return _purchaseOrderRepo.GetPurchaseOrders(searchTerm, pageNumber, pageSize);
        }

        public PurchaseOrderEntity GetPurchaseOrderById(int id)
        {
            return _purchaseOrderRepo.GetPurchaseOrderById(id);
        }

        public GenericResponse CreatePurchaseOrder(PurchaseOrderEntity purchaseOrder)
        {
            return _purchaseOrderRepo.CreatePurchaseOrder(purchaseOrder);
        }

        public GenericResponse UpdatePurchaseOrder(PurchaseOrderEntity purchaseOrder)
        {
            return _purchaseOrderRepo.UpdatePurchaseOrder(purchaseOrder);
        }

        public GenericResponse DeletePurchaseOrder(int id)
        {
            return _purchaseOrderRepo.DeletePurchaseOrder(id);
        }
    }
}
