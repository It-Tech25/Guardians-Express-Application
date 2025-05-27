using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services
{
    public class BankReceiptService : IBankReceiptService
    {
        private readonly IBankReceiptRepository _bankReceiptRepository;

        public BankReceiptService(IBankReceiptRepository bankReceiptRepository)
        {
            _bankReceiptRepository = bankReceiptRepository;
        }

        public List<VoucherDto> GetBankReceiptRegisterDetails(int? branchId, string fromDate, string toDate)
        {
            return _bankReceiptRepository.GetBankReceiptRegisterDetails(branchId, fromDate, toDate);
        }
    }
}