using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class GroupHeadRepo : IGroupHeadRepo
    {
        private readonly MyDbContext _context;

        public GroupHeadRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Group Head-------------------------------------------

        public IEnumerable<GroupHeadModel> GetGroupHeads(string searchTerm, int pageNumber, int pageSize)
        {
            // Retrieve all group head records that are not marked as deleted
            var groupHeads = _context.GroupHeads
                .Where(g => g.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)  // Implement pagination
                .Take(pageSize)
                .Select(g => new GroupHeadModel
                {
                    Id = g.Id,
                    GroupHeadName = g.GroupHeadName,
                    Behaviour = g.Behaviour,
                    OrderOfPLBs = g.OrderOfPLBs,
                    IsDeleted = g.IsDeleted
                }).ToList();

            return groupHeads;
        }

        public GenericResponse CreateGroupHead(GroupHeadModel groupHeadModel)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var groupHeadEntity = new GroupHeadEntity
                {
                    GroupHeadName = groupHeadModel.GroupHeadName,
                    Behaviour = groupHeadModel.Behaviour,
                    OrderOfPLBs = groupHeadModel.OrderOfPLBs,
                    IsDeleted = false // Set the default value for IsDeleted to false
                };

                _context.GroupHeads.Add(groupHeadEntity); // Add the group head entity to the context
                _context.SaveChanges(); // Save changes to the database
                response.statuCode = 1; // Success status code
                response.message = "Group Head created successfully"; // Success message
                response.currentId = groupHeadEntity.Id; // Return the ID of the created group head
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Group Head: " + ex.Message; // Error message
                response.currentId = 0; // Set the currentId to 0 in case of failure
            }
            return response;
        }

        public GroupHeadModel GetGroupHeadById(int id)
        {
            // Retrieve a group head by its ID and map it to GroupHeadModel
            var groupHead = _context.GroupHeads
                .Where(g => g.Id == id && g.IsDeleted == false)
                .FirstOrDefault();

            if (groupHead == null)
            {
                return null;
            }

            return new GroupHeadModel
            {
                Id = groupHead.Id,
                GroupHeadName = groupHead.GroupHeadName,
                Behaviour = groupHead.Behaviour,
                OrderOfPLBs = groupHead.OrderOfPLBs,
                IsDeleted = groupHead.IsDeleted
            };
        }

        public GenericResponse UpdateGroupHead(GroupHeadModel groupHeadModel)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing group head by its ID
                var existingGroupHead = _context.GroupHeads.Where(g => g.Id == groupHeadModel.Id && g.IsDeleted == false).FirstOrDefault();
                if (existingGroupHead != null)
                {
                    // Update the existing group head entity with the new values from GroupHeadModel
                    existingGroupHead.GroupHeadName = groupHeadModel.GroupHeadName;
                    existingGroupHead.Behaviour = groupHeadModel.Behaviour;
                    existingGroupHead.OrderOfPLBs = groupHeadModel.OrderOfPLBs;

                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1; // Success status code
                    response.message = "Group Head updated successfully"; // Success message
                    response.currentId = groupHeadModel.Id; // Return the ID of the updated group head
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Group Head not found"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the group head is not found
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Group Head: " + ex.Message; // Error message
                response.currentId = 0; // Set the currentId to 0 in case of failure
                return response;
            }
        }

        public GenericResponse DeleteGroupHead(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing group head by its ID
                var existingGroupHead = _context.GroupHeads.Where(g => g.Id == id && g.IsDeleted == false).FirstOrDefault();
                if (existingGroupHead != null)
                {
                    // Mark the group head as deleted
                    existingGroupHead.IsDeleted = true;
                    _context.Update(existingGroupHead); // Update the group head in the context
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1; // Success status code
                    response.message = "Group Head deleted successfully"; // Success message
                    response.currentId = id; // Return the ID of the deleted group head
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Group Head not found"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the group head is not found
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Group Head: " + ex.Message; // Error message
                response.currentId = 0; // Set the currentId to 0 in case of failure
            }
            return response;
        }
    }
}
