using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service;

public class CompanyConfigurationService : ICompanyConfigurationService
{
    private readonly ICompanyConfigurationRepo _companyConfigRepo;

    public CompanyConfigurationService(ICompanyConfigurationRepo companyConfigRepo)
    {
        _companyConfigRepo = companyConfigRepo;
    }

    public List<CompanyConfigurationViewModel> GetAllCompanyConfigurations(string searchTerm, int pageNumber, int pageSize)
    {
        return _companyConfigRepo.CompanyConfigurationList( searchTerm,  pageNumber, pageSize);
    }

    public CompanyConfigurationViewModel GetCompanyConfigurationById(int id)
    {
        return _companyConfigRepo.GetCompanyConfigurationById(id);
    }

    public GenericResponse CreateCompanyConfiguration(CompanyConfigurationViewModel model)
    {
        return _companyConfigRepo.AddCompanyConfiguration(model);
    }

    public GenericResponse UpdateCompanyConfiguration(CompanyConfigurationViewModel model)
    {
        return _companyConfigRepo.UpdateCompanyConfiguration(model);
    }

    public GenericResponse DeleteCompanyConfiguration(int id)
    {
        return _companyConfigRepo.DeleteCompanyConfiguration(id);
    }
    public async Task<GenericResponse> SaveCompany(string companyLogoPath, string letterHeadImagePath)
    {
        return await _companyConfigRepo.SaveCompany(companyLogoPath, letterHeadImagePath);
    }

}