using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IVehicleTypeRepo
    {
        IEnumerable<VehicleTypeDTO> GetVehicleTypes(string searchTerm, int pageNumber, int pageSize);

        GenericResponse CreateVehicleType(VehicleTypeDTO vehicleTypeDTO);

        VehicleTypeDTO GetVehicleTypeById(int id);

        GenericResponse UpdateVehicleType(VehicleTypeDTO vehicleTypeDTO);

        GenericResponse DeleteVehicleType(int id);

        Task<GenericResponse> SaveVehicleType(string vehicleImagePath);
    }
}
