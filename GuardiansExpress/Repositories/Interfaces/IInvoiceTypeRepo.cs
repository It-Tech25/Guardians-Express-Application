﻿using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IInvoiceTypeRepo
    {
        IEnumerable<InvoiceTypeModel> GetInvoiceTypes();
        InvoiceTypeModel GetInvoiceTypeById(int id);
        GenericResponse CreateInvoiceType(InvoiceTypeMasterEntity invoiceType);
        GenericResponse UpdateInvoiceType(InvoiceTypeMasterEntity invoiceType);
        GenericResponse DeleteInvoiceType(int id);
    }
}
