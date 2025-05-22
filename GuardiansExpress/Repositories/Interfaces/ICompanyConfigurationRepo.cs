using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ICompanyConfigurationRepo
    {
        List<CompanyConfigurationViewModel> CompanyConfigurationList(string searchTerm, int pageNumber, int pageSize);
        CompanyConfigurationViewModel GetCompanyConfigurationById(int id);
        GenericResponse AddCompanyConfiguration(CompanyConfigurationViewModel req);
        GenericResponse UpdateCompanyConfiguration(CompanyConfigurationViewModel req);
        GenericResponse DeleteCompanyConfiguration(int id);

        Task<GenericResponse> SaveCompany(string companyLogoPath, string letterHeadImagePath);
    }
}