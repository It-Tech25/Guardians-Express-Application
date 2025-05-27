using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repository.Interfaces
{
    public interface IBankReceiptRepository
    {
        List<VoucherDto> GetBankReceiptRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}