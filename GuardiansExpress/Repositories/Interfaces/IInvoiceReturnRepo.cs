using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IInvoiceReturnRepo
    {
        IEnumerable<InvoiceReturnModel> GetInvoiceReturns();
        IEnumerable<InvoiceReturnModel> GetInvoiceReturns(string searchTerm, int pageNumber, int pageSize);
        InvoiceReturnEntity GetInvoiceReturnById(int id);
        GenericResponse CreateInvoiceReturn(InvoiceReturnEntity invoiceReturn);
        GenericResponse UpdateInvoiceReturn(InvoiceReturnEntity invoiceReturn);
        GenericResponse DeleteInvoiceReturn(int id);
    }
}
