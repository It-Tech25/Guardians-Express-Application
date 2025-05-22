using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class VehicleGroupService : IVehicleGroupService
    {
        private readonly IVehicleGroupRepo _vehicleGroupRepo;

        public VehicleGroupService(IVehicleGroupRepo vehicleGroupRepo)
        {
            _vehicleGroupRepo = vehicleGroupRepo;
        }

        public IEnumerable<VehicleGroupModel> GetVehicleGroupMaster(string searchTerm, int pageNumber, int pageSize)
        {
            // Call repository method to get vehicle group list
            return _vehicleGroupRepo.GetVehicleGroupMaster(searchTerm, pageNumber, pageSize);
        }

        public GenericResponse CreateVehicleGroupMaster(VehicleGroupModel vehicleGroupModel)
        {
            // Call repository method to create a vehicle group
            return _vehicleGroupRepo.CreateVehicleGroupMaster(vehicleGroupModel);
        }

        public VehicleGroupModel GetVehicleGroupMasterById(int id)
        {
            // Call repository method to get a vehicle group by its ID
            return _vehicleGroupRepo.GetVehicleGroupMasterById(id);
        }

        public GenericResponse UpdateVehicleGroupMaster(VehicleGroupModel vehicleGroupModel)
        {
            // Call repository method to update a vehicle group
            return _vehicleGroupRepo.UpdateVehicleGroupMaster(vehicleGroupModel);
        }

        public GenericResponse DeleteVehicleGroupMaster(int id)
        {
            // Call repository method to delete a vehicle group
            return _vehicleGroupRepo.DeleteVehicleGroupMaster(id);
        }
    }
}
