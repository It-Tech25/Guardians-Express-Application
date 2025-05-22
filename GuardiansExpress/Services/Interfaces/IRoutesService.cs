using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IRoutesService
    {
        IEnumerable<Routes> GetRoutes(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateRoute(Routes route);
        Routes GetRouteById(int id);
        GenericResponse UpdateRoute(Routes route);
        GenericResponse DeleteRoute(int id);
    }
}
