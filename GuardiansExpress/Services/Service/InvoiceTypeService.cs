using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class InvoiceTypeService : IInvoiceTypeService
    {
        private readonly IInvoiceTypeRepo _invoiceTypeRepo;

        public InvoiceTypeService(IInvoiceTypeRepo invoiceTypeRepo)
        {
            _invoiceTypeRepo = invoiceTypeRepo;
        }

        public IEnumerable<InvoiceTypeModel> GetInvoiceTypes()
        {
            return _invoiceTypeRepo.GetInvoiceTypes();
        }

        public InvoiceTypeModel GetInvoiceTypeById(int id)
        {
            return _invoiceTypeRepo.GetInvoiceTypeById(id);
        }

        public GenericResponse CreateInvoiceType(InvoiceTypeMasterEntity invoiceType)
        {
            return _invoiceTypeRepo.CreateInvoiceType(invoiceType);
        }

        public GenericResponse UpdateInvoiceType(InvoiceTypeMasterEntity invoiceType)
        {
            return _invoiceTypeRepo.UpdateInvoiceType(invoiceType);
        }

        public GenericResponse DeleteInvoiceType(int id)
        {
            return _invoiceTypeRepo.DeleteInvoiceType(id);
        }
    }
}
