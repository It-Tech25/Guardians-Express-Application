using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class VehicleMasterService : IVehicleMasterService
    {
        private readonly IVehicleMastersRepo _Repo;

        public VehicleMasterService(IVehicleMastersRepo Repo)
        {
            _Repo = Repo;
        }

        public IEnumerable<VehicleMasterDTO> GetVehicleMaster(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {

                return _Repo.GetVehicleMaster(searchTerm, pageNumber, pageSize);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving Vehicle Master.", ex);
            }
        }
        public VehicleMasterEntity GetVehicleMasterById(int id)
        {
            try
            {

                return _Repo.GetVehicleMasterById(id);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving the Vehicle Master by ID.", ex);
            }
        }
        public GenericResponse CreateVehicleMaster(VehicleMasterEntity Vehicle)
        {
            try
            {

                return _Repo.CreateVehicleMaster(Vehicle);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the Vehicle Master.",
                    currentId = 0
                };
            }
        }
        public GenericResponse UpdateVehicleMaster(VehicleMasterEntity tax)
        {
            try
            {

                return _Repo.UpdateVehicleMaster(tax);
            }
            catch (Exception ex)
            {

                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the Vehicle Master.",
                    currentId = 0
                };
            }
        }
        public GenericResponse DeleteVehicleMaster(int id)
        {
            try
            {

                return _Repo.DeleteVehicleMaster(id);
            }
            catch (Exception ex)
            {

                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the Vehicle Master.",
                    currentId = 0
                };
            }

        }
    }
}
