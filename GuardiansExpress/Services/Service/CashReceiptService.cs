using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services
{
    public class CashReceiptService : ICashReceiptService
    {
        private readonly ICashReceiptRepository _cashReceiptRepository;

        public CashReceiptService(ICashReceiptRepository cashReceiptRegisterRepository)
        {
            _cashReceiptRepository = cashReceiptRegisterRepository;
        }

        public List<VoucherDto> GetCashReceiptRegisterDetails(int? branchId, string fromDate, string toDate)
        {
            return _cashReceiptRepository.GetCashReceiptRegisterDetails(branchId, fromDate, toDate);
        }
    }
}