using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

public class BranchMasterController : Controller
{
    private readonly IBranchMasterService _service;
    private readonly ICompanyConfigurationService _companySettingsService;
    private readonly ICountryAndStateService _cnssrv;
    private readonly MyDbContext _context;

    public BranchMasterController(IBranchMasterService service, ICompanyConfigurationService companySettingsService, ICountryAndStateService cnssrv, MyDbContext context)
    {
        _service = service;
        _companySettingsService = companySettingsService;
        _cnssrv = cnssrv;
        _context = context;
    }

    public IActionResult BranchMasterIndex()
    {
        var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
        if (loggedInUser == null)
        {
            return RedirectToAction("Login", "Authenticate");
        }

        ViewBag.UserName = loggedInUser.userName;
        ViewBag.UserEmail = loggedInUser.Emailid;
        ViewBag.UserRole = loggedInUser.Role;

        string searchTerm = "";
        int pageNumber = 1;
        int pageSize = 10;

        var branches = _service.BranchMaster(searchTerm, pageNumber, pageSize);

        var branchDTOs = branches.Select(res => new BranchMasterDTO
        {
            id = res.id,
            Company = res.Company,
            BranchName = res.BranchName,
            CompanyName = res.CompanyName,
            StateName = res.StateName,
            Email = res.Email,
            Address = res.Address,
            City = res.City,
            Country = res.Country,
            Pincode = res.Pincode,
            POC = res.POC,
            StampImage = res.StampImage,
            status = res.status
        }).ToList();

        var cc = _companySettingsService.GetAllCompanyConfigurations(searchTerm, pageNumber, pageSize).ToList();
        var c = _cnssrv.Country().ToList();
        var s = _cnssrv.StatebyContry().ToList();

        ViewBag.cc = cc;
        ViewBag.c = c;
        ViewBag.s = s;

        return View(branchDTOs);
    }

    [HttpPost]
    public IActionResult Save(BranchMasterDTO model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Invalid data." });
        }

        BranchMasterEntity branchEntity;
        GenericResponse response;

        if (model.id == 0 || model.id == null)
        {
            branchEntity = new BranchMasterEntity
            {
                CreatedOn = DateTime.Now,
                CreatedBy = model.CreatedBy
            };
        }
        else
        {
            branchEntity = _context.branch.FirstOrDefault(b => b.id == model.id);
            if (branchEntity == null)
            {
                return Json(new { success = false, message = "Branch not found." });
            }
            branchEntity.UpdatedOn = DateTime.Now;
            branchEntity.UpdatedBy = model.UpdatedBy;
        }

        if (model.StampImg != null && model.StampImg.Length > 0)
        {
            var stampFilePath = Path.Combine("wwwroot/uploadImages", model.StampImg.FileName);
            using (var stream = new FileStream(stampFilePath, FileMode.Create))
            {
                model.StampImg.CopyTo(stream);
            }
            branchEntity.StampImage = "/uploads/" + model.StampImg.FileName;
        }

        branchEntity.BranchName = model.BranchName;
        branchEntity.Company = model.Company;
        branchEntity.Email = model.Email;
        branchEntity.POC = model.POC;
        branchEntity.BranchCode = model.BranchCode;
        branchEntity.Address = model.Address;
        branchEntity.City = model.City;
        branchEntity.StateId = model.StateId;
        branchEntity.Country = model.Country;
        branchEntity.Pincode = model.Pincode;
        branchEntity.IsActive = model.IsActive ?? true;
        branchEntity.IsDeleted = model.IsDeleted ?? false;
        branchEntity.status = model.status ?? false;

        if (model.id == 0 || model.id == null)
        {
            response = _service.CreateBranchMaster(branchEntity);
        }
        else
        {
            response = _service.UpdateBranchMaster(branchEntity);
        }

        if (response.statuCode == 1)
        {
            var updatedBranches = _service.BranchMaster("", 1, 10).ToList();
            return Json(new { success = true, message = "Successfully Added.", data = updatedBranches });
        }
        else
        {
            return Json(new { success = false, message = response.message });
        }
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        if (id == 0)
        {
            return BadRequest("Invalid Branch ID.");
        }

        var response = _service.DeleteBranchMaster(id);
        if (response.statuCode == 1)
        {
            var updatedBranches = _service.BranchMaster("", 1, 10).ToList();
            return Json(new { success = true, data = updatedBranches });
        }
        else
        {
            return Json(new { success = false, message = response.message });
        }
    }
} 
