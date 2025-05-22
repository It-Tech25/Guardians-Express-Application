using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class VehicleTypeRepo : IVehicleTypeRepo
    {
        private readonly MyDbContext _context;

        public VehicleTypeRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Vehicle Type Master-------------------------------------------

        public IEnumerable<VehicleTypeDTO> GetVehicleTypes(string searchTerm, int pageNumber, int pageSize)
        {
            var vehicleTypes = _context.VehicleTypeEntite
                .Where(vt => vt.IsDeleted == false &&
                            (string.IsNullOrEmpty(searchTerm) || vt.VehicleType.Contains(searchTerm)))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(vt => new VehicleTypeDTO
                {
                    Id = vt.Id,
                    VehicleType = vt.VehicleType,
                    VehicleImage = vt.VehicleImage,
                    IsActive = vt.IsActive,
                    IsDeleted = vt.IsDeleted,
                    CreatedOn = vt.CreatedOn,
                    CreatedBy = vt.CreatedBy,
                    UpdatedOn = vt.UpdatedOn,
                    UpdatedBy = vt.UpdatedBy
                })
                .ToList();
            return vehicleTypes;
        }

        public GenericResponse CreateVehicleType(VehicleTypeDTO vehicleTypeDTO)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var vehicleTypeEntity = new VehicleTypeEntity
                {
                    VehicleType = vehicleTypeDTO.VehicleType,
                    VehicleImage = vehicleTypeDTO.VehicleImage,
                    IsActive = vehicleTypeDTO.IsActive,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedBy = vehicleTypeDTO.CreatedBy,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = vehicleTypeDTO.UpdatedBy
                };

                _context.VehicleTypeEntite.Add(vehicleTypeEntity);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Vehicle Type created successfully";
                response.currentId = vehicleTypeEntity.Id;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Vehicle Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public VehicleTypeDTO GetVehicleTypeById(int id)
        {
            var vehicleType = _context.VehicleTypeEntite
                .Where(vt => vt.Id == id && vt.IsDeleted == false)
                .FirstOrDefault();

            if (vehicleType != null)
            {
                return new VehicleTypeDTO
                {
                    Id = vehicleType.Id,
                    VehicleType = vehicleType.VehicleType,
                    VehicleImage = vehicleType.VehicleImage,
                    IsActive = vehicleType.IsActive,
                    IsDeleted = vehicleType.IsDeleted,
                    CreatedOn = vehicleType.CreatedOn,
                    CreatedBy = vehicleType.CreatedBy,
                    UpdatedOn = vehicleType.UpdatedOn,
                    UpdatedBy = vehicleType.UpdatedBy
                };
            }
            return null;
        }

        public GenericResponse UpdateVehicleType(VehicleTypeDTO vehicleTypeDTO)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingVehicleType = _context.VehicleTypeEntite
                    .Where(vt => vt.Id == vehicleTypeDTO.Id && vt.IsDeleted == false)
                    .FirstOrDefault();

                if (existingVehicleType != null)
                {
                    existingVehicleType.VehicleType = vehicleTypeDTO.VehicleType;
                    existingVehicleType.VehicleImage = vehicleTypeDTO.VehicleImage;
                    existingVehicleType.IsActive = vehicleTypeDTO.IsActive;
                    existingVehicleType.UpdatedOn = DateTime.Now;
                    existingVehicleType.UpdatedBy = vehicleTypeDTO.UpdatedBy;
                    existingVehicleType.IsDeleted = false;

                    _context.Update(existingVehicleType);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Vehicle Type updated successfully";
                    response.currentId = vehicleTypeDTO.Id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Vehicle Type not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Vehicle Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeleteVehicleType(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingVehicleType = _context.VehicleTypeEntite
                    .Where(vt => vt.Id == id && vt.IsDeleted == false)
                    .FirstOrDefault();

                if (existingVehicleType != null)
                {
                    existingVehicleType.IsDeleted = true;
                    _context.Update(existingVehicleType);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Vehicle Type deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Vehicle Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
        public async Task<GenericResponse> SaveVehicleType(string vehicleImagePath)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var vehicleTypeEntity = new VehicleTypeEntity
                {
                    VehicleImage = vehicleImagePath,
                    CreatedOn = DateTime.Today,
                    IsDeleted = false
                };

                _context.VehicleTypeEntite.Add(vehicleTypeEntity);
                await _context.SaveChangesAsync();

                response.statuCode = 1;
                response.message = "Vehicle type saved successfully.";
                response.currentId = vehicleTypeEntity.Id;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to save vehicle type.";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

    }
}
