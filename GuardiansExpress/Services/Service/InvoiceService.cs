using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepo _invoiceRepo;

        public InvoiceService(IInvoiceRepo invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        public IEnumerable<InvoiceDTO> GetInvoices(string searchTerm, int pageNumber, int pageSize)
        {
            return _invoiceRepo.GetInvoice(searchTerm, pageNumber, pageSize);
        }

        public InvoiceEntity GetInvoiceById(int id)
        {
            return _invoiceRepo.GetInvoiceById(id);
        }

        public GenericResponse CreateInvoice(InvoiceEntity invoice)
        {
            try
            {
                // Add any business logic validations here
                invoice.CreatedOn = DateTime.Now;
                invoice.UpdatedOn = DateTime.Now;
                invoice.IsActive = true;

                return _invoiceRepo.createInvoice(invoice);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "Service Error: " + ex.Message,
                    currentId = 0
                };
            }
        }

        public GenericResponse UpdateInvoice(InvoiceEntity invoice)
        {
            try
            {
                // Add any business logic validations here
                invoice.UpdatedOn = DateTime.Now;

                return _invoiceRepo.UpdateInvoice(invoice);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "Service Error: " + ex.Message,
                    currentId = 0
                };
            }
        }

        public GenericResponse DeleteInvoice(int id)
        {
            try
            {
                return _invoiceRepo.DeleteInvoice(id);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "Service Error: " + ex.Message,
                    currentId = 0
                };
            }
        }
    }
}