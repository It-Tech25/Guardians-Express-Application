using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Repositories.Repos;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class TaxMasterService: ITaxMasterService
    {

        private readonly ITaxMaster _tmasterrepo;

        // Constructor
        public TaxMasterService(ITaxMaster tmasterrepo)
        {
            _tmasterrepo = tmasterrepo;
        }
        public IEnumerable<TaxMasterDto> GetTaxMastersWithLedger(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                // Retrieve company setup data from the repository
                return _tmasterrepo.GetTaxMastersWithLedger(searchTerm, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return empty list if needed
                throw new Exception("An error occurred while retrieving Tax data.", ex);    
            }
        }

        public GenericResponse CreateTaxMaster(TaxMasterDto Tax)
        {
            try
            {
                // Create a company setup using the repository
                return _tmasterrepo.CreateTaxMaster(Tax);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the Tax.",
                    currentId = 0
                };
            }
        }

        //public TaxMasterDto GetTaxMasterById(int id)
        //{
        //    try
        //    {
        //        // Retrieve a company setup by ID
        //        return _tmasterrepo.GetTaxMasterById(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error and rethrow or return null if needed
        //        throw new Exception("An error occurred while retrieving the Tax by ID.", ex);
        //    }
        //}

        public GenericResponse UpdateTaxMaster(TaxMasterDto Tax)
        {
            try
            {
                // Update the Tax using the repository
                return _tmasterrepo.UpdateTaxMaster(Tax);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the Tax.",
                    currentId = 0
                };
            }
        }

        public GenericResponse DeleteTaxMaster(int id)
        {
            try
            {
                // Delete the Tax using the repository
                return _tmasterrepo.DeleteTaxMaster(id);
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the Tax.",
                    currentId = 0
                };
            }
        }
    }
}

