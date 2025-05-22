using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IGroupHeadRepo
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
