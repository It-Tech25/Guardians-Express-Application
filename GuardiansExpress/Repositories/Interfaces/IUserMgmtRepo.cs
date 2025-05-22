using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IUserMgmtRepo
    {
        LoginResponse LoginCheck(LoginRequest request);
        public IEnumerable<UsersMasterDTO> UserMasterEntity(string searchTerm, int pageNumber, int pageSize);
        public UsersMasterDTO UserMasterById(int id);
        public GenericResponse CreateUserMaster(UserMaster res);
        public GenericResponse UpdateUserType(UserMaster update);
        public GenericResponse DeleteUserType(int id);
    }
}
