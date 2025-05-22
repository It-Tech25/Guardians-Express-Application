using GuardiansExpress.Models.DTO;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IGroupHeadService
    {
        // Get a list of GroupHeads with pagination and search
        IEnumerable<GroupHeadModel> GetGroupHeads(string searchTerm, int pageNumber, int pageSize);

        // Create a new GroupHead
        GenericResponse CreateGroupHead(GroupHeadModel groupHeadModel);

        // Get a GroupHead by its ID
        GroupHeadModel GetGroupHeadById(int id);

        // Update an existing GroupHead
        GenericResponse UpdateGroupHead(GroupHeadModel groupHeadModel);

        // Mark a GroupHead as deleted
        GenericResponse DeleteGroupHead(int id);
    }
}
