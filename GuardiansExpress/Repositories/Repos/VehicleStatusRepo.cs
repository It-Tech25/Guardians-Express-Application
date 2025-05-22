using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories.Repos
{
    public class VehicleStatusRepo : IVehicleStatusRepo
    {
        private readonly MyDbContext _context;

        public VehicleStatusRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<VehicleStatusEntity> GetVehicleStatuses()
        {
            return _context.vehicleStatuses.ToList();
        }

        public GenericResponse CreateVehicleStatus(VehicleStatusEntity vehicleStatus)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                _context.vehicleStatuses.Add(vehicleStatus);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Vehicle status created successfully";
                response.currentId = vehicleStatus.VehicleStatusID;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save vehicle status: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public VehicleStatusEntity GetVehicleStatusById(int id)
        {
            return _context.vehicleStatuses.Find(id);
        }

        public GenericResponse UpdateVehicleStatus(VehicleStatusEntity vehicleStatus)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingStatus = _context.vehicleStatuses.Find(vehicleStatus.VehicleStatusID);
                if (existingStatus != null)
                {
                   
                    _context.Entry(existingStatus).CurrentValues.SetValues(vehicleStatus);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Vehicle status updated successfully";
                    response.currentId = vehicleStatus.VehicleStatusID;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Vehicle status not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update vehicle status: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeleteVehicleStatus(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingStatus = _context.vehicleStatuses.Find(id);
                if (existingStatus != null)
                {
                    _context.vehicleStatuses.Remove(existingStatus);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Vehicle status deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Vehicle status not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete vehicle status: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
