using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IVehicleTypeService
    {
        IEnumerable<VehicleTypeDTO> GetVehicleTypes(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateVehicleType(VehicleTypeDTO vehicleTypeDTO);
        VehicleTypeDTO GetVehicleTypeById(int id);
        GenericResponse UpdateVehicleType(VehicleTypeDTO vehicleTypeDTO);
        GenericResponse DeleteVehicleType(int id);
        Task<GenericResponse> SaveVehicleType(string vehicleImagePath);

    }
}
