using GuardiansExpress.Models.DTO;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class GroupHeadService : IGroupHeadService
    {
        private readonly IGroupHeadRepo _groupHeadRepo;

        public GroupHeadService(IGroupHeadRepo groupHeadRepo)
        {
            _groupHeadRepo = groupHeadRepo;
        }

        public IEnumerable<GroupHeadModel> GetGroupHeads(string searchTerm, int pageNumber, int pageSize)
        {
            // Delegate the call to the repository layer
            return _groupHeadRepo.GetGroupHeads(searchTerm, pageNumber, pageSize);
        }

        public GenericResponse CreateGroupHead(GroupHeadModel groupHeadModel)
        {
            // Delegate the call to the repository layer
            return _groupHeadRepo.CreateGroupHead(groupHeadModel);
        }

        public GroupHeadModel GetGroupHeadById(int id)
        {
            // Delegate the call to the repository layer
            return _groupHeadRepo.GetGroupHeadById(id);
        }

        public GenericResponse UpdateGroupHead(GroupHeadModel groupHeadModel)
        {
            // Delegate the call to the repository layer
            return _groupHeadRepo.UpdateGroupHead(groupHeadModel);
        }

        public GenericResponse DeleteGroupHead(int id)
        {
            // Delegate the call to the repository layer
            return _groupHeadRepo.DeleteGroupHead(id);
        }
    }
}
