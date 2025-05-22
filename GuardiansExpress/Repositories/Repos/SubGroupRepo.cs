using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GuardiansExpress.Repositories.Repos
{
    public class SubGroupHeadRepo : ISubGroupHeadRepo
    {
        private readonly MyDbContext _context;

        public SubGroupHeadRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Group Head Master-------------------------------------------
        public IEnumerable<GroupHeadEntity> GroupHeadMaster()
        {
            var groupHeads = _context.GroupHeads.Where(a => a.IsDeleted == false).ToList();
            return groupHeads;
        }
        public IEnumerable<SubGroupEntity> SubGroupHeadMaster()
        {
             var groupHeads = _context.SubGroups.Where(a => a.IsDeleted == false).ToList();
            return groupHeads;
        }

        public IEnumerable<SubGroupHeadDTO> SubGroupHeadList()
        {
            var groupHeads =from subgroup in _context.SubGroups
                            join Group in _context.GroupHeads on subgroup.GroupId equals Group.Id into GroupJoin
                        from Group in GroupJoin.DefaultIfEmpty()
                        where (subgroup == null || (subgroup.IsDeleted == false))
                        select new SubGroupHeadDTO
                        {
                           subgroupId= subgroup.subgroupId,
                            SubGroupName = subgroup.SubGroupName,
                            Group = Group != null ? Group.GroupHeadName : null,
                            Behaviour = Group != null ? Group.Behaviour : null,
                            Detailed = subgroup.Detailed,
                            AcceptAddress = subgroup.AcceptAddress,
                            Employee = subgroup.Employee,
                            BalanceDashboard = subgroup.BalanceDashboard,
                            orderin = subgroup.orderin,
                            GroupId = Group != null ? Group.Id : null,
                        };
            return groupHeads;
        }

        public GenericResponse CreateSubGroupHeadMaster(SubGroupEntity groupHead)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Check if the subgroup with the same name already exists
                var existingGroupHead = _context.SubGroups
                    .Where(a => a.SubGroupName == groupHead.SubGroupName && a.IsDeleted==false) // Replace SubGroupName with the field that should be unique
                    .FirstOrDefault();

                if (existingGroupHead != null)
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "A Sub Group Head with the same name already exists."; // Error message
                    response.currentId = 0; // No ID returned since creation failed
                    return response; // Exit early if a duplicate is found
                }

                // Set default values for the new subgroup
                groupHead.IsActive = true;
                groupHead.IsDeleted = false;

                // Add the new subgroup to the context
                _context.SubGroups.Add(groupHead);

                // Save changes to the database
                _context.SaveChanges();

                // Return success response
                response.statuCode = 1;
                response.message = "Sub Group Head created successfully";
                response.currentId = groupHead.subgroupId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Sub Group Head: " + ex.Message;
                response.currentId = 0; // Set the currentId to 0 in case of failure
            }
            return response;
        }


        public SubGroupEntity GetSubGroupHeadById(int id)
        {
            return _context.SubGroups.Find(id);
        }

        public GenericResponse UpdateSubGroupHeadMaster(SubGroupEntity groupHead)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Check if a subgroup with the same name already exists (excluding the current group head being updated)
                var existingGroupHeadWithSameName = _context.SubGroups
                    .Where(a => a.SubGroupName == groupHead.SubGroupName && a.subgroupId != groupHead.subgroupId) // Exclude current group being updated
                    .FirstOrDefault();

                if (existingGroupHeadWithSameName != null)
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "A Sub Group Head with the same name already exists."; // Error message
                    response.currentId = 0; // Set the currentId to 0 if update fails due to duplicate
                    return response; // Exit early if a duplicate is found
                }

                // Find the existing subgroup by its ID
                var existingGroupHead = _context.SubGroups
                    .Where(a => a.subgroupId == groupHead.subgroupId)
                    .FirstOrDefault();

                if (existingGroupHead != null)
                {
                    // Update the existing group's properties
                    existingGroupHead.SubGroupName = groupHead.SubGroupName;
                    existingGroupHead.GroupId = groupHead.GroupId;
                    existingGroupHead.Detailed = groupHead.Detailed;
                    existingGroupHead.AcceptAddress = groupHead.AcceptAddress;
                    existingGroupHead.Employee = groupHead.Employee;
                    existingGroupHead.BalanceDashboard = groupHead.BalanceDashboard;
                    existingGroupHead.orderin = groupHead.orderin;
                    existingGroupHead.IsActive = true;
                    existingGroupHead.IsDeleted = false;
                    existingGroupHead.UpdatedOn = DateTime.Now;

                    // Update the existing group in the context
                    _context.SubGroups.Update(existingGroupHead);
                    _context.SaveChanges();

                    response.statuCode = 1; // Success status code
                    response.message = "Sub Group Head updated successfully"; // Success message
                    response.currentId = groupHead.subgroupId; // Return the ID of the updated subgroup
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Sub Group Head not found"; // Error message if the subgroup is not found
                    response.currentId = 0; // Set the currentId to 0 if the subgroup is not found
                }

                return response;
            }
            catch (Exception ex)
            {
                response.statuCode = 0; // Failure status code
                response.message = "Failed to update Sub Group Head: " + ex.Message; // Error message
                response.currentId = 0; // Set the currentId to 0 in case of failure
                return response;
            }
        }


        public GenericResponse DeleteSubGroupHeadMaster(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingGroupHead = _context.SubGroups.Where(a => a.subgroupId == id).FirstOrDefault();
                if (existingGroupHead != null)
                {
                    existingGroupHead.IsDeleted = true;

                    _context.Update(existingGroupHead);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Sub Group Head deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Sub Group Head: " + ex.Message;
                response.currentId = 0;
                return response;
            }
        }
    }
}
