using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IInvoiceRepo
    {
        IEnumerable<InvoiceDTO> GetInvoice(string searchTerm, int pageNumber, int pageSize);
        InvoiceEntity GetInvoiceById(int id);
        GenericResponse createInvoice(InvoiceEntity Invoice);
        GenericResponse UpdateInvoice(InvoiceEntity invoice);
        GenericResponse DeleteInvoice(int id);
    }
}