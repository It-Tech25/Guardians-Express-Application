using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services.Service
{
    public class InvoiceRegisterService : IInvoiceRegisterService
    {
        private readonly IInvoiceRegisterRepository _invoiceRegisterRepository;

        public InvoiceRegisterService(IInvoiceRegisterRepository invoiceRegisterRepository)
        {
            _invoiceRegisterRepository = invoiceRegisterRepository;
        }

        public List<InvoiceDTO> GetInvoiceRegisterDetails(int? branchId, string fromDate, string toDate)
        {
            return _invoiceRegisterRepository.GetInvoiceRegisterDetails(branchId, fromDate, toDate);
        }
    }
}