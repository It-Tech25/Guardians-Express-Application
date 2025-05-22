using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IRoutesRepo
    {
        IEnumerable<Routes> GetRoutes(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateRoute(Routes route);
        Routes GetRouteById(int id);
        GenericResponse UpdateRoute(Routes route);
        GenericResponse DeleteRoute(int id);
    }
}
