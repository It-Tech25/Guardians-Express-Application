using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IGRService
    {
        IEnumerable<GRDTOs> Getgrdetails();
        GenericResponse AddAsync(GRDTOs grDto, string serializedinvoiceData);
        GenericResponse UpdateAsync(GRDTOs grDto, string serializedinvoiceData);
        GenericResponse DeleteAsync(int id);
    }
}
