using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IVehicleGroupRepo
    {
        IEnumerable<VehicleGroupModel> GetVehicleGroupMaster(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateVehicleGroupMaster(VehicleGroupModel vehicleGroupModel);
        VehicleGroupModel GetVehicleGroupMasterById(int id);
        GenericResponse UpdateVehicleGroupMaster(VehicleGroupModel vehicleGroupModel);
        GenericResponse DeleteVehicleGroupMaster(int id);
    }
}
