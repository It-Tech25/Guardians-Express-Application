// IInvoiceService.cs
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IInvoiceService
    {
        IEnumerable<InvoiceDTO> GetInvoices(string searchTerm, int pageNumber, int pageSize);
        InvoiceEntity GetInvoiceById(int id);
        GenericResponse CreateInvoice(InvoiceEntity invoice);
        GenericResponse UpdateInvoice(InvoiceEntity invoice);
        GenericResponse DeleteInvoice(int id);
    }
}