using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IVehicleTypeRepo _vehicleTypeRepo;

        public VehicleTypeService(IVehicleTypeRepo vehicleTypeRepo)
        {
            _vehicleTypeRepo = vehicleTypeRepo;
        }

        public IEnumerable<VehicleTypeDTO> GetVehicleTypes(string searchTerm, int pageNumber, int pageSize)
        {
            return _vehicleTypeRepo.GetVehicleTypes(searchTerm, pageNumber, pageSize);
        }

        public GenericResponse CreateVehicleType(VehicleTypeDTO vehicleTypeDTO)
        {
            return _vehicleTypeRepo.CreateVehicleType(vehicleTypeDTO);
        }

        public VehicleTypeDTO GetVehicleTypeById(int id)
        {
            return _vehicleTypeRepo.GetVehicleTypeById(id);
        }

        public GenericResponse UpdateVehicleType(VehicleTypeDTO vehicleTypeDTO)
        {
            return _vehicleTypeRepo.UpdateVehicleType(vehicleTypeDTO);
        }

        public GenericResponse DeleteVehicleType(int id)
        {
            return _vehicleTypeRepo.DeleteVehicleType(id);
        }

        public Task<GenericResponse> SaveVehicleType(string vehicleImagePath)
        {
            return _vehicleTypeRepo.SaveVehicleType(vehicleImagePath);
        }
    }
}
