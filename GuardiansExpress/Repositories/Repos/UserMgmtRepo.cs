using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories.Repos
{
    public class UserMgmtRepo : IUserMgmtRepo
    {
        private readonly MyDbContext _context;

        public UserMgmtRepo(MyDbContext context)
        {
            _context = context;
        }
        public LoginResponse LoginCheck(LoginRequest request)
        {
            LoginResponse lr = new LoginResponse();

            try
            {
                var u = _context.users
                    .Where(a => a.IsDeleted==false && a.IsActive==true && a.EmailId == request.emailId)
                    .FirstOrDefault();

                if (u == null)
                {
                    return new LoginResponse
                    {
                        statusCode = 0,
                        Message = "EmailId not registered / not active"
                    };
                }
                        lr.statusCode = 1;
                        lr.Message = "success";
                        lr.userTypeName = _context.userTypes.Where(a => a.UserTypeId == u.UserTypeId).Select(b => b.UserTypeName).FirstOrDefault();
                        lr.userName = u.UserName;
                        lr.userId = u.UserId;
                        lr.Role = u.Role;
                        lr.Emailid = u.EmailId;
                var decryptedPassword = EncryptModel.Decrypt(u.Password);

                if (request.password == decryptedPassword)
                {
                    return new LoginResponse
                    {
                        statusCode = 1,
                        Message = "Login success",
                        userTypeName = _context.userTypes.Where(a => a.UserTypeId == u.UserTypeId).Select(b => b.UserTypeName).FirstOrDefault(),
                        userName = u.UserName,
                        userId = u.UserId
                    };
                }

                return new LoginResponse
                {
                    statusCode = 0,
                    Message = "Password incorrect"
                };
            }
            catch (Exception ex)
            {
                // Log the exception (replace with your logging mechanism)
                Console.WriteLine($"LoginCheck Error: {ex.Message}");

                return new LoginResponse
                {
                    statusCode = -1,
                    Message = "An error occurred while processing your request."
                };
            }
        }
        public IEnumerable<UsersMasterDTO> UserMasterEntity(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from users in _context.users
                        join usertype in _context.userTypes on users.UserTypeId equals usertype.UserTypeId into usertypejoin
                        from usertype in usertypejoin.DefaultIfEmpty()
                        where users.IsDeleted == false && users.IsActive == true
                        select new UsersMasterDTO
                        {
                            UserId = users.UserId,
                            UserName = users.UserName,
                            UserTypeName = usertype != null ? usertype.UserTypeName : null,
                            EmailId = users.EmailId,
                            City = users.City,
                            State = users.State,
                            Password = users.Password,
                            MobileNumber = users.MobileNumber,
                            Pincode = users.Pincode,
                            AadharCardBackUrl = users.AadharCardBackUrl,
                            AadharCardFrontUrl = users.AadharCardFrontUrl,
                            Address = users.Address,
                            Country = users.Country,
                            CurrentStatus = users.CurrentStatus,
                            CreatedBy = users.CreatedBy,
                            CreatedOn = users.CreatedOn,
                            UpdatedBy = users.UpdatedBy,
                            UpdateOn = users.UpdateOn,
                            Role = users.Role,
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(t =>
                    t.UserName.ToLower().Contains(searchTerm) ||
                    (t.Role != null && t.Role.ToLower().Contains(searchTerm)) ||
                    (t.UserTypeName != null && t.UserTypeName.ToLower().Contains(searchTerm)) ||
                    (t.CurrentStatus != null && t.CurrentStatus.ToLower().Contains(searchTerm)) ||
                    (t.EmailId != null && t.EmailId.ToLower().Contains(searchTerm)));
            }

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public UsersMasterDTO UserMasterById(int id)
        {
            var query = (from users in _context.users
                         join usertype in _context.userTypes on users.UserTypeId equals usertype.UserTypeId into usertypejoin
                         from usertype in usertypejoin.DefaultIfEmpty()
                         where (users == null || (users.IsDeleted == false && users.IsActive == true))
                         select new UsersMasterDTO
                         {
                             UserId = users.UserId,
                             UserName=users.UserName,
                             UserTypeName = usertype != null ? usertype.UserTypeName : null,
                             EmailId = users.EmailId,
                             City = users.City,
                             State = users.State,
                             MobileNumber=users.MobileNumber,
                             Password = users.Password,
                             Pincode = users.Pincode,
                             AadharCardBackUrl = users.AadharCardBackUrl,
                             AadharCardFrontUrl = users.AadharCardFrontUrl,
                             Address = users.Address,
                             Country = users.Country,
                             CurrentStatus = users.CurrentStatus,
                             CreatedBy = users.CreatedBy,
                             CreatedOn = users.CreatedOn,
                             UpdatedBy = users.UpdatedBy,
                             UpdateOn = users.UpdateOn,
                             Role = users.Role,

                         }).FirstOrDefault();

            return query;
        }
        public GenericResponse CreateUserMaster(UserMaster res)
        {
            GenericResponse response = new GenericResponse();

            int count = _context.users.Where(a => a.Role == res.Role && a.IsDeleted == false).Count();
            if (count < 1)
            {
                try
                {
                    var UserTypeId = _context.userTypes
                        .Where(a => a.UserTypeName == res.Role && a.IsDeleted == false)
                        .Select(a => a.UserTypeId)
                        .FirstOrDefault();

                    if (UserTypeId == 0)  // Ensure subgroup exists
                    {
                        response.message = "Error: In Adding The User.";
                        return response;
                    }

                    UserMaster T = new UserMaster
                    {
                        UserTypeId = UserTypeId,
                        UserName = res.UserName,
                        EmailId = res.EmailId,
                        City = res.City,
                        MobileNumber = res.MobileNumber,

                        State = res.State,
                        Password = res.Password,
                        Pincode = res.Pincode,
                        AadharCardBackUrl = res.AadharCardBackUrl,
                        AadharCardFrontUrl = res.AadharCardFrontUrl,
                        Address = res.Address,
                        Country = res.Country,
                        IsActive=true,
                        IsDeleted=false,
                        CurrentStatus = res.CurrentStatus,
                        CreatedBy = res.UserId,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = res.UserId,
                        UpdateOn = DateTime.Now,
                        Role = res.Role,
                    };

                    _context.users.Add(T);
                    _context.SaveChanges();

                    // ✅ Make sure to return the LedgerId after saving
                    response.statuCode = 1;
                    response.message = "User created successfully";
                    response.currentId = T.UserId;  // ✅ Return the correct LedgerId

                }
                catch (Exception ex)
                {
                    response.message = "Failed to save UserMaster: " + ex.Message;
                }
            }
            else
            {
                response.message = "UserMaster already exists Or Enter The Empty Inputs.";
            }
            return response;
        }
        public GenericResponse UpdateUserType(UserMaster update)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingUserType = _context.users
                    .Where(vt => vt.UserId == update.UserId  && vt.IsDeleted == false)
                    .FirstOrDefault();

                if (existingUserType != null)
                {
                    existingUserType.UserName = update.UserName;
                    existingUserType.EmailId = update.EmailId;
                    existingUserType.IsActive = update.IsActive;
                    existingUserType.UpdateOn = DateTime.Now;
                    existingUserType.AadharCardBackUrl = update.AadharCardBackUrl;
                    existingUserType.AadharCardFrontUrl= update.AadharCardFrontUrl;
                    existingUserType.UpdatedBy = update.UserId;
                    existingUserType.IsDeleted = false;
                    existingUserType.IsActive = true;
                    existingUserType.EmailId = update.EmailId;
                    existingUserType.MobileNumber =update.MobileNumber;
                    existingUserType.City = update.City;
                    existingUserType.Country = update.Country;
                    existingUserType.Pincode = update.Pincode;
                    existingUserType.Password = update.Password;
                    existingUserType.Role= update.Role;
                    existingUserType.CurrentStatus = update.CurrentStatus;  
                    _context.users.Update(existingUserType);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "UserMaster  updated successfully";
                    response.currentId = update.UserId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "UserMaster Type not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update UserMaster: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeleteUserType(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {

                var entity = _context.users.FirstOrDefault(x => x.UserId == id);

                entity.IsDeleted = true;
                entity.IsActive = false;
                _context.Update(entity);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Delete Successful.";

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Delete failed.";
                //response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
