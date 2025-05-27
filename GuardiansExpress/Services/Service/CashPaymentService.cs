using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services
{
    public class CashPaymentService : ICashPaymentService
    {
        private readonly ICashPaymentRepository _cashPaymentRepository;

        public CashPaymentService(ICashPaymentRepository cashPaymentRepository)
        {
            _cashPaymentRepository = cashPaymentRepository;
        }

        public List<VoucherDto> GetCashPaymentRegisterDetails(int? branchId, string fromDate, string toDate)
        {
            return _cashPaymentRepository.GetCashPaymentRegisterDetails(branchId, fromDate, toDate);
        }
    }
}