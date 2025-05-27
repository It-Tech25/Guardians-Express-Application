using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Interfaces
{
    public interface ICashPaymentService
    {
        List<VoucherDto> GetCashPaymentRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}