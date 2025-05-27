using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IBankReceiptService
    {
        List<VoucherDto> GetBankReceiptRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}