using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        IEnumerable<PurchaseOrderModel> GetPurchaseOrders(string searchTerm, int pageNumber, int pageSize);
        PurchaseOrderEntity GetPurchaseOrderById(int id);
        GenericResponse CreatePurchaseOrder(PurchaseOrderEntity purchaseOrder);
        GenericResponse UpdatePurchaseOrder(PurchaseOrderEntity purchaseOrder);
        GenericResponse DeletePurchaseOrder(int id);
    }
}
