using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
    public class Routesrepo : IRoutesRepo  // Ensure it implements the interface
    {
        private readonly MyDbContext _context;

        public Routesrepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Company Setup Master-------------------------------------------

        public IEnumerable<Routes> GetRoutes(string searchTerm, int pageNumber, int pageSize)
        {
            var routes = _context.routes
                .Where(r => r.IsDelete == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return routes;
        }

        public GenericResponse CreateRoute(Routes route)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                route.IsDelete = false;
                route.IsActive = true;
                route.CreatedOn = DateTime.Now;

                _context.routes.Add(route);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Route created successfully";
                response.currentId = route.id;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Route: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public Routes GetRouteById(int id)
        {
            return _context.routes.Find(id);
        }

        public GenericResponse UpdateRoute(Routes route)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingRoute = _context.routes.FirstOrDefault(r => r.id == route.id);
                if (existingRoute != null)
                {
                    _context.Entry(existingRoute).CurrentValues.SetValues(route);
                    existingRoute.UpdatedOn = DateTime.Now;

                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Route updated successfully";
                    response.currentId = route.id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Route not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Route: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeleteRoute(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingRoute = _context.routes.FirstOrDefault(r => r.id == id);
                if (existingRoute != null)
                {
                    existingRoute.IsDelete = true;
                    _context.Update(existingRoute);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Route deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Route: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
