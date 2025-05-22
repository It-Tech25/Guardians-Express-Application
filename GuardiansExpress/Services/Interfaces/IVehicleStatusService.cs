using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IVehicleStatusService
    {
        IEnumerable<VehicleStatusEntity> GetAllVehicleStatuses();
        GenericResponse AddVehicleStatus(VehicleStatusEntity vehicleStatus);
        VehicleStatusEntity GetVehicleStatus(int id);
        GenericResponse UpdateVehicleStatus(VehicleStatusEntity vehicleStatus);
        GenericResponse RemoveVehicleStatus(int id);
    }
}
