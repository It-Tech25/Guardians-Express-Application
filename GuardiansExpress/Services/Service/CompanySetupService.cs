using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services
{
    public class CompanySetupService : ICompanySetupService
    {
        private readonly ICompanySetupRepo _companySetupRepo;

        public CompanySetupService(ICompanySetupRepo companySetupRepo)
        {
            _companySetupRepo = companySetupRepo;
        }

        public IEnumerable<CompanySetupMasterEntity> GetCompanySetupMaster(string searchTerm , int pageNumber , int pageSize )
        {
            try
            {
                // Retrieve company setup data from the repository
                return _companySetupRepo.GetCompanySetupMaster(searchTerm, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return empty list if needed
                throw new Exception("An error occurred while retrieving company setup data.", ex);
            }
        }

        public GenericResponse CreateCompanySetupMaster(CompanySetupMasterEntity companySetup)
        {
            try
            {
                // Create a company setup using the repository
                return _companySetupRepo.CreateCompanySetupMaster(companySetup);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the company setup.",
                    currentId = 0
                };
            }
        }

        public CompanySetupMasterEntity GetCompanySetupMasterById(int id)
        {
            try
            {
                // Retrieve a company setup by ID
                return _companySetupRepo.GetCompanySetupMasterById(id);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return null if needed
                throw new Exception("An error occurred while retrieving the company setup by ID.", ex);
            }
        }

        public GenericResponse UpdateCompanySetupMaster(CompanySetupMasterEntity companySetup)
        {
            try
            {
                // Update the company setup using the repository
                return _companySetupRepo.UpdateCompanySetupMaster(companySetup);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the company setup.",
                    currentId = 0
                };
            }
        }

        public GenericResponse DeleteCompanySetupMaster(int id)
        {
            try
            {
                // Delete the company setup using the repository
                return _companySetupRepo.DeleteCompanySetupMaster(id);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the company setup.",
                    currentId = 0
                };
            }
        }
    }
}
