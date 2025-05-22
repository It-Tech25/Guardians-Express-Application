using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IUserTypeMasterService
    {
        IEnumerable<UserTypeMasterDto> GetUserTypeMasters(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateUserTypeMaster(UserTypeMasterDto userType);
        UserTypeMasterDto GetUserTypeMasterById(int id);
        GenericResponse UpdateUserTypeMaster(UserTypeMasterDto userType);
        GenericResponse DeleteUserTypeMaster(int id);
    }
}