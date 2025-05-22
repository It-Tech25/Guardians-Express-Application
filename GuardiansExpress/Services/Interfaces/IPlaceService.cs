using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IPlaceService
    {
        IEnumerable<Place> GetAllPlaces(string searchTerm, int pageNumber, int pageSize);
        Place GetPlace(int id);
        GenericResponse AddPlace(Place place);
        GenericResponse EditPlace(Place place);
        GenericResponse RemovePlace(int id);
    }
}
