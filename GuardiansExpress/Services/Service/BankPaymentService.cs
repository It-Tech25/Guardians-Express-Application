using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services
{
    public class BankPaymentService : IBankPaymentService
    {
        private readonly IBankPaymentRepository _bankPaymentRepository;

        public BankPaymentService(IBankPaymentRepository bankPaymentRepository)
        {
            _bankPaymentRepository = bankPaymentRepository;
        }

        public List<VoucherDto> GetBankPaymentRegisterDetails(int? branchId, string fromDate, string toDate)
        {
            return _bankPaymentRepository.GetBankPaymentRegisterDetails(branchId, fromDate, toDate);
        }
    }
}