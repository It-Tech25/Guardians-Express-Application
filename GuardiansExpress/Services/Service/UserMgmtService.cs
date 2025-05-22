using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class UserMgmtService : IUserMgmtService
    {
        private readonly IUserMgmtRepo repo;
        public UserMgmtService(IUserMgmtRepo userMgmtRepo)
        {
            repo = userMgmtRepo;
        }

        public LoginResponse LoginCheck(LoginRequest request)
        {
            return repo.LoginCheck(request);
        }
        public IEnumerable<UsersMasterDTO> UserMasterEntity(string searchTerm, int pageNumber, int pageSize)
        {
            return repo.UserMasterEntity(searchTerm, pageNumber, pageSize);
        }
        public UsersMasterDTO UserMasterById(int id)
        {
            return repo.UserMasterById(id);
        }
        public GenericResponse CreateUserMaster(UserMaster res)
        {
            return repo.CreateUserMaster(res);
        }
       
        public GenericResponse UpdateUserType(UserMaster update)
        {
            return repo.UpdateUserType(update);
        }
        public GenericResponse DeleteUserType(int id)
        {
            return repo.DeleteUserType(id);
        }
    }
}
