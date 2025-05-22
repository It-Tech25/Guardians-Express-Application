using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IGRTypeService
    {
        IEnumerable<GRType> GetAllGRTypes();
        IEnumerable<GRType> GetAllGRTypes(string searchTerm, int pageNumber, int pageSize);
        GRType GetGRType(int id);
        GenericResponse AddGRType(GRType grType);
        GenericResponse EditGRType(GRType grType);
        GenericResponse RemoveGRType(int id);
    }
}