// ICompanyConfigurationService.cs
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface ICompanyConfigurationService
    {
        List<CompanyConfigurationViewModel> GetAllCompanyConfigurations(string searchTerm, int pageNumber, int pageSize);
        CompanyConfigurationViewModel GetCompanyConfigurationById(int id);
        GenericResponse CreateCompanyConfiguration(CompanyConfigurationViewModel model);
        GenericResponse UpdateCompanyConfiguration(CompanyConfigurationViewModel model);
        GenericResponse DeleteCompanyConfiguration(int id);
        Task<GenericResponse> SaveCompany(string companyLogoPath, string letterHeadImagePath);
    }
}