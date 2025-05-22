using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IGRTypeRepo
    {
        IEnumerable<GRType> GetGRTypes(string searchTerm, int pageNumber, int pageSize);
        GRType GetGRTypeById(int id);
        GenericResponse CreateGRType(GRType grType);
        GenericResponse UpdateGRType(GRType grType);
        GenericResponse DeleteGRType(int id);
    }
}