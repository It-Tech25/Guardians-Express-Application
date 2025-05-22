using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ISubGroupHeadRepo
    {
        IEnumerable<GroupHeadEntity> GroupHeadMaster();
        IEnumerable<SubGroupEntity> SubGroupHeadMaster();
        IEnumerable<SubGroupHeadDTO> SubGroupHeadList();
        GenericResponse CreateSubGroupHeadMaster(SubGroupEntity groupHead);
        SubGroupEntity GetSubGroupHeadById(int id);
        GenericResponse UpdateSubGroupHeadMaster(SubGroupEntity groupHead);
        GenericResponse DeleteSubGroupHeadMaster(int id);


    }
}
