using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class SubGroupHeadService : ISubGroupHeadService
    {
        private readonly ISubGroupHeadRepo _subGroupHeadRepo;

        public SubGroupHeadService(ISubGroupHeadRepo subGroupHeadRepo)
        {
            _subGroupHeadRepo = subGroupHeadRepo;
        }
       public IEnumerable<GroupHeadEntity> GroupHeadMaster()
        {
            return _subGroupHeadRepo.GroupHeadMaster();
        }
        public IEnumerable<SubGroupEntity> GetSubGroupHeadMaster()
        {
            return _subGroupHeadRepo.SubGroupHeadMaster();
        }
       public IEnumerable<SubGroupHeadDTO> SubGroupHeadList()
        {
            return _subGroupHeadRepo.SubGroupHeadList();
        }

        public GenericResponse CreateSubGroupHeadMaster(SubGroupEntity subGroupHead)
        {
            return _subGroupHeadRepo.CreateSubGroupHeadMaster(subGroupHead);
        }

        public SubGroupEntity GetSubGroupHeadById(int id)
        {
            return _subGroupHeadRepo.GetSubGroupHeadById(id);
        }

        public GenericResponse UpdateSubGroupHeadMaster(SubGroupEntity subGroupHead)
        {
            return _subGroupHeadRepo.UpdateSubGroupHeadMaster(subGroupHead);
        }

        public GenericResponse DeleteSubGroupHeadMaster(int id)
        {
            return _subGroupHeadRepo.DeleteSubGroupHeadMaster(id);
        }
    }
}
