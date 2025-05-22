using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IPlaceRepo
    {
        IEnumerable<Place> GetPlaces(string searchTerm, int pageNumber, int pageSize);
        Place GetPlaceById(int id);
        GenericResponse CreatePlace(Place place);
        GenericResponse UpdatePlace(Place place);
        GenericResponse DeletePlace(int id);
    }
}
