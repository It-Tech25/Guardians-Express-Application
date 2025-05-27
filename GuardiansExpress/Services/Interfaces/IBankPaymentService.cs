using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IBankPaymentService
    {
        List<VoucherDto> GetBankPaymentRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}