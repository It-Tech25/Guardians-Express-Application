using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface ISubGroupHeadService
    {
        // Get a list of all SubGroup entities
        IEnumerable<GroupHeadEntity> GroupHeadMaster();
        IEnumerable<SubGroupEntity> GetSubGroupHeadMaster();
        IEnumerable<SubGroupHeadDTO> SubGroupHeadList();

        // Create a new SubGroup entity
        GenericResponse CreateSubGroupHeadMaster(SubGroupEntity subGroupHead);

        // Get a SubGroup entity by its ID
        SubGroupEntity GetSubGroupHeadById(int id);

        // Update an existing SubGroup entity
        GenericResponse UpdateSubGroupHeadMaster(SubGroupEntity subGroupHead);

        // Delete a SubGroup entity by marking it as deleted (soft delete)
        GenericResponse DeleteSubGroupHeadMaster(int id);
    }
}
