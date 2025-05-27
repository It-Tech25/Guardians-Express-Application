using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repository.Interfaces
{
    public interface ICashPaymentRepository
    {
        List<VoucherDto> GetCashPaymentRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}