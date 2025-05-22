using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IVehicleMasterService
    {
        IEnumerable<VehicleMasterDTO> GetVehicleMaster(string searchTerm, int pageNumber, int pageSize);
        VehicleMasterEntity GetVehicleMasterById(int id);
        GenericResponse CreateVehicleMaster(VehicleMasterEntity Vehicle);
        GenericResponse UpdateVehicleMaster(VehicleMasterEntity tax);
        GenericResponse DeleteVehicleMaster(int id);

    }
}
