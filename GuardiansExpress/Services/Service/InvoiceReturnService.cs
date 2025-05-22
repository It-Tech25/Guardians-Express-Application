using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class InvoiceReturnService : IInvoiceReturnService
    {
        private readonly IInvoiceReturnRepo _invoiceReturnRepo;

        public InvoiceReturnService(IInvoiceReturnRepo invoiceReturnRepo)
        {
            _invoiceReturnRepo = invoiceReturnRepo;
        }
       public IEnumerable<InvoiceReturnModel> GetInvoiceReturns()
        {
            return _invoiceReturnRepo.GetInvoiceReturns();
        }
        public IEnumerable<InvoiceReturnModel> GetInvoiceReturns(string searchTerm, int pageNumber, int pageSize)
        {
            return _invoiceReturnRepo.GetInvoiceReturns(searchTerm, pageNumber, pageSize);
        }

        public InvoiceReturnEntity GetInvoiceReturnById(int id)
        {
            return _invoiceReturnRepo.GetInvoiceReturnById(id);
        }

        public GenericResponse CreateInvoiceReturn(InvoiceReturnEntity invoiceReturn)
        {
            return _invoiceReturnRepo.CreateInvoiceReturn(invoiceReturn);
        }

        public GenericResponse UpdateInvoiceReturn(InvoiceReturnEntity invoiceReturn)
        {
            return _invoiceReturnRepo.UpdateInvoiceReturn(invoiceReturn);
        }

        public GenericResponse DeleteInvoiceReturn(int id)
        {
            return _invoiceReturnRepo.DeleteInvoiceReturn(id);
        }
    }
}
