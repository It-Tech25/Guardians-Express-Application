using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IInvoiceTypeService
    {
        IEnumerable<InvoiceTypeModel> GetInvoiceTypes();
        InvoiceTypeModel GetInvoiceTypeById(int id);
        GenericResponse CreateInvoiceType(InvoiceTypeMasterEntity invoiceType);
        GenericResponse UpdateInvoiceType(InvoiceTypeMasterEntity invoiceType);
        GenericResponse DeleteInvoiceType(int id);
    }
}
