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
    public class VehicleGroupRepo : IVehicleGroupRepo
    {
        private readonly MyDbContext _context;

        public VehicleGroupRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Vehicle Group Master-------------------------------------------

        public IEnumerable<VehicleGroupModel> GetVehicleGroupMaster(string searchTerm, int pageNumber, int pageSize)
        {
            // Retrieve all vehicle group records that are not marked as deleted
            var vehicleGroups = _context.vehicles
                .Where(vg => vg.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(vg => new VehicleGroupModel
                {
                    Id = vg.Id,
                    VehicleGroup = vg.VehicleGroup,
                    IsActive = vg.IsActive,
                    IsDeleted = vg.IsDeleted,
                    CreatedOn = vg.CreatedOn,
                    CreatedBy = vg.CreatedBy,
                    UpdatedOn = vg.UpdatedOn,
                    UpdatedBy = vg.UpdatedBy
                })
                .ToList();
            return vehicleGroups;
        }

        public GenericResponse CreateVehicleGroupMaster(VehicleGroupModel vehicleGroupModel)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Convert model to entity
                var vehicleGroupEntity = new VehicleGroupEntity
                {
                    VehicleGroup = vehicleGroupModel.VehicleGroup,
                    IsActive = vehicleGroupModel.IsActive,
                    IsDeleted = false, // Set the default value for IsDeleted to false
                    CreatedOn = DateTime.Now,
                    CreatedBy = vehicleGroupModel.CreatedBy,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = vehicleGroupModel.UpdatedBy
                };

                _context.vehicles.Add(vehicleGroupEntity); // Add the vehicle group entity to the context
                _context.SaveChanges();  // Save changes to the database
                response.statuCode = 1;  // Success status code
                response.message = "Vehicle Group created successfully"; // Success message
                response.currentId = vehicleGroupEntity.Id; // Return the ID of the created vehicle group
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Vehicle Group: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }

        public VehicleGroupModel GetVehicleGroupMasterById(int id)
        {
            // Retrieve a vehicle group by its ID and convert it to the VehicleGroupModel
            var vehicleGroup = _context.vehicles
                .Where(vg => vg.Id == id && vg.IsDeleted == false)
                .FirstOrDefault();

            if (vehicleGroup != null)
            {
                return new VehicleGroupModel
                {
                    Id = vehicleGroup.Id,
                    VehicleGroup = vehicleGroup.VehicleGroup,
                    IsActive = vehicleGroup.IsActive,
                    IsDeleted = vehicleGroup.IsDeleted,
                    CreatedOn = vehicleGroup.CreatedOn,
                    CreatedBy = vehicleGroup.CreatedBy,
                    UpdatedOn = vehicleGroup.UpdatedOn,
                    UpdatedBy = vehicleGroup.UpdatedBy
                };
            }
            return null; // Return null if not found
        }

        public GenericResponse UpdateVehicleGroupMaster(VehicleGroupModel vehicleGroupModel)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing vehicle group by its ID
                var existingVehicleGroup = _context.vehicles
                    .Where(vg => vg.Id == vehicleGroupModel.Id && vg.IsDeleted == false)
                    .FirstOrDefault();

                if (existingVehicleGroup != null)
                {
                    // Update the existing vehicle group entity with the new values
                    existingVehicleGroup.VehicleGroup = vehicleGroupModel.VehicleGroup;
                    existingVehicleGroup.IsActive = vehicleGroupModel.IsActive;
                    existingVehicleGroup.UpdatedOn = DateTime.Now;
                    existingVehicleGroup.UpdatedBy = vehicleGroupModel.UpdatedBy;
                    existingVehicleGroup.IsDeleted = false;

                    _context.Update(existingVehicleGroup); // Update the vehicle group in the context
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1;  // Success status code
                    response.message = "Vehicle Group updated successfully"; // Success message
                    response.currentId = vehicleGroupModel.Id; // Return the ID of the updated vehicle group
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Vehicle Group not found"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the vehicle group is not found
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Vehicle Group: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }

        public GenericResponse DeleteVehicleGroupMaster(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing vehicle group by its ID
                var existingVehicleGroup = _context.vehicles
                    .Where(vg => vg.Id == id && vg.IsDeleted == false)
                    .FirstOrDefault();

                if (existingVehicleGroup != null)
                {
                    // Mark the vehicle group as deleted
                    existingVehicleGroup.IsDeleted = true;
                    _context.Update(existingVehicleGroup); // Update the vehicle group in the context
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1; // Success status code
                    response.message = "Vehicle Group deleted successfully"; // Success message
                    response.currentId = id; // Return the ID of the deleted vehicle group
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Delete Failed"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the vehicle group is not found
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Vehicle Group: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }
    }
}
