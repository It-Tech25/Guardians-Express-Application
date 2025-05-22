using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IPurchaseDetailsService
    {
        IEnumerable<PurchaseDetailsDTO> GetPurchaseDetails(string searchTerm, int pageNumber, int pageSize);
        PurchaseDetailsDTO GetPurchaseDetailById(int id);
        GenericResponse CreatePurchaseDetail(PurchaseDetailsDTO purchaseDetailDto);
        GenericResponse UpdatePurchaseDetail(PurchaseDetailsDTO purchaseDetailDto);
        GenericResponse DeletePurchaseDetail(int id);
        IEnumerable<PurchaseDetailsEntity> GetPurchaselists();
    }
}
