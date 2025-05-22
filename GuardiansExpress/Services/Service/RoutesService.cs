using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Repositories.Repos;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class RoutesService : IRoutesService
    {
        private readonly IRoutesRepo _routesRepo;

        public RoutesService(IRoutesRepo routesRepo)
        {
            _routesRepo = routesRepo;
        }

        public IEnumerable<Routes> GetRoutes(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                // Retrieve routes data from the repository
                return _routesRepo.GetRoutes(searchTerm, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return an empty list if needed
                throw new Exception("An error occurred while retrieving routes data.", ex);
            }
        }

        public GenericResponse CreateRoute(Routes route)
        {
            try
            {
                // Create a route using the repository
                return _routesRepo.CreateRoute(route);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the route.",
                    currentId = 0
                };
            }
        }

        public Routes GetRouteById(int id)
        {
            try
            {
                // Retrieve a route by ID
                return _routesRepo.GetRouteById(id);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return null if needed
                throw new Exception("An error occurred while retrieving the route by ID.", ex);
            }
        }

        public GenericResponse UpdateRoute(Routes route)
        {
            try
            {
                // Update the route using the repository
                return _routesRepo.UpdateRoute(route);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the route.",
                    currentId = 0
                };
            }
        }

        public GenericResponse DeleteRoute(int id)
        {
            try
            {
                // Delete the route using the repository
                return _routesRepo.DeleteRoute(id);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the route.",
                    currentId = 0
                };
            }
        }
    }
}
