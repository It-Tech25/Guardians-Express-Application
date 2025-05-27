using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repository.Interfaces
{
    public interface ICashReceiptRepository
    {
        List<VoucherDto> GetCashReceiptRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}