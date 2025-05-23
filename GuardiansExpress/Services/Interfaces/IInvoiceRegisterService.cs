using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IInvoiceRegisterService
    {
        List<InvoiceDTO> GetInvoiceRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}