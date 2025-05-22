using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GuardiansExpress.Repositories.Repos
{
    public class UserTypeMasterRepo : IUserTypeMasterRepo
    {
        private readonly MyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserTypeMasterRepo(MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            // Attempt to get user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            // Fallback to default user ID if no claims found
            return 1; // Default system user
        }

        
          public UserTypeMasterDto GetUserTypeMasterById(int id)
        {
            var query = _context.userTypes
                .Where(userType => !userType.IsDeleted && userType.UserTypeId==id && userType.IsActive)
                .Select(userType => new UserTypeMasterDto
                {
                    UserTypeId = userType.UserTypeId,
                    UserTypeName = userType.UserTypeName,
                    Description = userType.Discription,
                    IsActive = userType.IsActive,
                    IsDeleted = userType.IsDeleted
                })
            .FirstOrDefault();
            return query;
        }



        public IEnumerable<UserTypeMasterDto> GetUserTypeMasters(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.userTypes
                .Where(userType => !userType.IsDeleted && userType.IsActive)
                .Select(userType => new UserTypeMasterDto
                {
                    UserTypeId = userType.UserTypeId,
                    UserTypeName = userType.UserTypeName,
                    Description = userType.Discription,
                    IsActive = userType.IsActive,
                    IsDeleted = userType.IsDeleted
                });

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u =>
                    u.UserTypeName.Contains(searchTerm) ||
                    u.Description.Contains(searchTerm));
            }

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public GenericResponse CreateUserTypeMaster(UserTypeMasterDto userType)
        {
            var response = new GenericResponse();

            // Validate input
            if (string.IsNullOrWhiteSpace(userType.UserTypeName))
            {
                response.statuCode = 0;
                response.message = "User Type Name is required";
                return response;
            }

            // Check for existing user type
            bool exists = _context.userTypes
                .Any(u => u.UserTypeName.Trim().ToLower() == userType.UserTypeName.Trim().ToLower()
                          && !u.IsDeleted);

            if (exists)
            {
                response.statuCode = 0;
                response.message = "User Type Name already exists";
                return response;
            }

            try
            {
                var newUserType = new UserTypeMaster
                {
                    UserTypeName = userType.UserTypeName.Trim(),
                    Discription = userType.Description?.Trim() ?? string.Empty,
                    IsDeleted = false,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = GetCurrentUserId()
                };

                _context.userTypes.Add(newUserType);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "User Type created successfully";
                response.currentId = newUserType.UserTypeId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = $"Failed to create User Type: {ex.Message}";
            }

            return response;
        }

        public GenericResponse UpdateUserTypeMaster(UserTypeMasterDto userType)
        {
            var response = new GenericResponse();

            if (userType.UserTypeId <= 0)
            {
                response.statuCode = 0;
                response.message = "Invalid User Type ID";
                return response;
            }

            try
            {
                var existingUserType = _context.userTypes
                    .FirstOrDefault(u => u.UserTypeId == userType.UserTypeId && !u.IsDeleted);

                if (existingUserType == null)
                {
                    response.statuCode = 0;
                    response.message = "User Type not found";
                    return response;
                }

                // Check if name is already in use by another user type
                bool nameExists = _context.userTypes
                    .Any(u => u.UserTypeName.Trim().ToLower() == userType.UserTypeName.Trim().ToLower()
                              && u.UserTypeId != userType.UserTypeId
                              && !u.IsDeleted);

                if (nameExists)
                {
                    response.statuCode = 0;
                    response.message = "User Type Name already exists";
                    return response;
                }

                existingUserType.UserTypeName = userType.UserTypeName.Trim();
                existingUserType.Discription = userType.Description?.Trim() ?? string.Empty;
                existingUserType.UpdatedOn = DateTime.UtcNow;
                existingUserType.UpdatedBy = GetCurrentUserId();
                existingUserType.IsActive = userType.IsActive;

                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "User Type updated successfully";
                response.currentId = existingUserType.UserTypeId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = $"Failed to update User Type: {ex.Message}";
            }

            return response;
        }

        public GenericResponse DeleteUserTypeMaster(int id)
        {
            var response = new GenericResponse();

            if (id <= 0)
            {
                response.statuCode = 0;
                response.message = "Invalid User Type ID";
                return response;
            }

            try
            {
                var existingUserType = _context.userTypes
                    .FirstOrDefault(u => u.UserTypeId == id && !u.IsDeleted);

                if (existingUserType == null)
                {
                    response.statuCode = 0;
                    response.message = "User Type not found";
                    return response;
                }

                existingUserType.IsDeleted = true;
                existingUserType.UpdatedOn = DateTime.UtcNow;
                existingUserType.UpdatedBy = GetCurrentUserId();

                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "User Type deleted successfully";
                response.currentId = id;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = $"Failed to delete User Type: {ex.Message}";
            }

            return response;
        }
    }
}