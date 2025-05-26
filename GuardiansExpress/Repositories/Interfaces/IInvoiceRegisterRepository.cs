using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repository.Interfaces
{
    public interface IInvoiceRegisterRepository
    {
        List<InvoiceDTO> GetInvoiceRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}