using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ICompanySetupRepo
    {
        IEnumerable<CompanySetupMasterEntity> GetCompanySetupMaster(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateCompanySetupMaster(CompanySetupMasterEntity companySetup);
        CompanySetupMasterEntity GetCompanySetupMasterById(int id);
        GenericResponse UpdateCompanySetupMaster(CompanySetupMasterEntity companySetup);
        GenericResponse DeleteCompanySetupMaster(int id);
    }
}
