using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IPurchaseDetailsRepo
    {
        IEnumerable<PurchaseDetailsDTO> GetPurchaseDetails();
        PurchaseDetailsDTO GetPurchaseById(int id);
        GenericResponse CreatePurchase(PurchaseDetailsDTO purchaseDto);
        GenericResponse UpdatePurchase(PurchaseDetailsDTO purchaseDto);
        GenericResponse DeletePurchase(int id);
        IEnumerable<PurchaseDetailsEntity> GetPurchaselists();
    }
}
