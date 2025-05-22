using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;

public class BranchMasterRepo : IBranchMasterRepo
{

    private readonly MyDbContext _context;


    public BranchMasterRepo(MyDbContext context)
    {
        _context = context;
    }



    public IEnumerable<BranchMasterEntity> BranchMasterss()
    {
        var entity = _context.branch.Where(x => x.IsDeleted == false).ToList();
        return entity;
    }

        public IEnumerable<BranchMasterDTO> BranchMaster(string searchTerm, int pageNumber, int pageSize) 
        {
        var query = from b in _context.branch
                    where b.IsDeleted == false // Ensure BranchMaster is not deleted
                    select new BranchMasterDTO
                    {
                        id = b.id,
                        CompanyName = _context.CompanyConfigurations.Where(a=>a.CompanyId==b.Company).Select(a=>a.Name).FirstOrDefault(),
                        StateName = _context.stateEntities.Where(a => a.Id == b.StateId).Select(a => a.StateName).FirstOrDefault(),
                        BranchName = b.BranchName,
                        status = b.status,
                        BranchCode = b.BranchCode,
                        Email = b.Email,
                        Address = b.Address,
                        City = b.City,
                        StateId = b.StateId,
                        Country = b.Country,
                        Pincode = b.Pincode,
                        POC = b.POC,
                        StampImage = b.StampImage,
                    };

        return query.Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();

    }


    public BranchMasterEntity BranchMasterId(int id)
        {
            var entity = _context.branch.Where(x => x.id == id && x.IsDeleted == false).FirstOrDefault();
            return entity;
          
        }
    public GenericResponse CreateBranchMaster(BranchMasterEntity res)
    {
        GenericResponse checkStatus = new GenericResponse();
        try
        {

            BranchMasterEntity response = new BranchMasterEntity
            {
                Company = res.Company,
                BranchName = res.BranchName,
                Email = res.Email,
                Address = res.Address,
                City = res.City,
                StateId = res.StateId,
                Country = res.Country,
                Pincode = res.Pincode,
                POC = res.POC,
                StampImage = res.StampImage,
                IsDeleted = false,
                CreatedBy = res.CreatedBy,
                CreatedOn = DateTime.Now,
                IsActive = true,
                status=res.status

            };

            _context.branch.Add(response);
            _context.SaveChanges();

            checkStatus.statuCode = 1;
            checkStatus.message = "Add Successful.";
            checkStatus.currentId = 0;
        }
        catch (Exception ex)
        {
            checkStatus.statuCode = 0;
            checkStatus.message = "Add failed";
            checkStatus.ErrorMessage = ex.Message;
        }
        return checkStatus;
    }
    public GenericResponse UpdateBranchName(BranchMasterEntity req)
    {
        GenericResponse response = new GenericResponse();
        try
        {
            var u = _context.branch.Where(a => a.id == req.id).FirstOrDefault();
            if (u == null)
            {
                response.message = "Record not found";
                response.currentId = 0;
                return response;
            }
            _context.Entry(u).State = EntityState.Detached;
            req.IsActive = true;
            req.IsDeleted = false;
            req.UpdatedOn = DateTime.Now;
            _context.branch.Update(req);
            _context.SaveChanges();
            response.statuCode = 1;
            response.message = "Success";
            response.currentId = req.id;
            return response;
        }
        catch (Exception ex)
        {

            response.message = "Failed to update: " + ex.Message;
            response.currentId = 0;
            return response;
        }
    }
    public GenericResponse DeleteBranchName(int id)
    {
        GenericResponse response = new GenericResponse();
        try
        {
            var entity = _context.branch.FirstOrDefault(x => x.id == id);

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
            response.ErrorMessage = ex.Message;
        }
        return response;
    }
}

