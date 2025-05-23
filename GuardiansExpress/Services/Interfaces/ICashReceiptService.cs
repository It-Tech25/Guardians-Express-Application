using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Interfaces
{
    public interface ICashReceiptService
    {
        List<VoucherDto> GetCashReceiptRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}