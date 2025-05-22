using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IVehicleStatusRepo
    {
        IEnumerable<VehicleStatusEntity> GetVehicleStatuses();
        GenericResponse CreateVehicleStatus(VehicleStatusEntity vehicleStatus);
        VehicleStatusEntity GetVehicleStatusById(int id);
        GenericResponse UpdateVehicleStatus(VehicleStatusEntity vehicleStatus);
        GenericResponse DeleteVehicleStatus(int id);
    }
}
