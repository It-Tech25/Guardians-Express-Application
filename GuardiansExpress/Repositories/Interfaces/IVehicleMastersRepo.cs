using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IVehicleMastersRepo
    {
        IEnumerable<VehicleMasterDTO> GetVehicleMaster(string searchTerm, int pageNumber, int pageSize);
        VehicleMasterEntity GetVehicleMasterById(int id);
        GenericResponse CreateVehicleMaster(VehicleMasterEntity Vehicle);
        GenericResponse UpdateVehicleMaster(VehicleMasterEntity Vehicle);
        GenericResponse DeleteVehicleMaster(int id);

    }
}
