using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repository.Interfaces
{
    public interface IBankPaymentRepository
    {
        List<VoucherDto> GetBankPaymentRegisterDetails(int? branchId, string fromDate, string toDate);
    }
}