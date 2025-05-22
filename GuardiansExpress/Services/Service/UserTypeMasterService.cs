using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Repositories.Repos;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class UserTypeMasterService : IUserTypeMasterService
    {
        private readonly IUserTypeMasterRepo _umasterrepo;

        // Constructor
        public UserTypeMasterService(IUserTypeMasterRepo umasterrepo)
        {
            _umasterrepo = umasterrepo;
        }

        public IEnumerable<UserTypeMasterDto> GetUserTypeMasters(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                // Retrieve user type data from the repository
                return _umasterrepo.GetUserTypeMasters(searchTerm, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return empty list if needed
                throw new Exception("An error occurred while retrieving User Type data.", ex);
            }
        }

        public GenericResponse CreateUserTypeMaster(UserTypeMasterDto userType)
        {
            try
            {
                // Create a user type using the repository
                return _umasterrepo.CreateUserTypeMaster(userType);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the User Type.",
                    currentId = 0
                };
            }
        }

        public UserTypeMasterDto GetUserTypeMasterById(int id)
        {
            try
            {
                // Retrieve a user type by ID
                return _umasterrepo.GetUserTypeMasterById(id);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return null if needed
                throw new Exception("An error occurred while retrieving the User Type by ID.", ex);
            }
        }

        public GenericResponse UpdateUserTypeMaster(UserTypeMasterDto userType)
        {
            try
            {
                // Update the User Type using the repository
                return _umasterrepo.UpdateUserTypeMaster(userType);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the User Type.",
                    currentId = 0
                };
            }
        }

        public GenericResponse DeleteUserTypeMaster(int id)
        {
            try
            {
                // Delete the User Type using the repository
                return _umasterrepo.DeleteUserTypeMaster(id);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the User Type.",
                    currentId = 0
                };
            }
        }
    }
}