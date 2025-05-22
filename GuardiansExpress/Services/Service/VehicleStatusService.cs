using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

public class VehicleStatusService : IVehicleStatusService
{
    private readonly IVehicleStatusRepo _vehicleStatusRepo;

    public VehicleStatusService(IVehicleStatusRepo vehicleStatusRepo)
    {
        _vehicleStatusRepo = vehicleStatusRepo;
    }

    public IEnumerable<VehicleStatusEntity> GetAllVehicleStatuses()
    {   
        return _vehicleStatusRepo.GetVehicleStatuses();
    }

    public GenericResponse AddVehicleStatus(VehicleStatusEntity vehicleStatus)
    {
        return _vehicleStatusRepo.CreateVehicleStatus(vehicleStatus);
    }

    public VehicleStatusEntity GetVehicleStatus(int id)
    {
        return _vehicleStatusRepo.GetVehicleStatusById(id);
    }

    public GenericResponse UpdateVehicleStatus(VehicleStatusEntity vehicleStatus)
    {
        return _vehicleStatusRepo.UpdateVehicleStatus(vehicleStatus);
    }

    public GenericResponse RemoveVehicleStatus(int id)
    {
        return _vehicleStatusRepo.DeleteVehicleStatus(id);
    }
}