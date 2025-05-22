using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IGRRepo
    {
        IEnumerable<GRDTOs> Getgrdetails();
        GenericResponse AddAsync(GRDTOs grDto, string serializedinvoiceData);
        GenericResponse UpdateAsync(GRDTOs grDto, string serializedinvoiceData);
        GenericResponse DeleteAsync(int id);
    }
}
