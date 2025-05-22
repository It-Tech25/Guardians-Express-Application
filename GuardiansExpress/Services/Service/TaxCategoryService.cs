using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Repositories.Repos;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class TaxCategoryService : ITaxCategoryService
    {
        private readonly ITaxCategoryRepo _Repo;

        public TaxCategoryService(ITaxCategoryRepo Repo)
        {
            _Repo = Repo;
        }
        public IEnumerable<TaxCategoryEntity> GetTaxCategory(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {

                return _Repo.GetTaxCategory(searchTerm, pageNumber, pageSize);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving tax category.", ex);
            }
        }

        public TaxCategoryEntity GetTaxCategoryById(int id)
        {
            try
            {

                return _Repo.GetTaxCategoryById(id);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving the tax category by ID.", ex);
            }
        }

        public GenericResponse CreateTaxCategory(TaxCategoryEntity tax,
     List<int> TaxMasterID,
     List<decimal> Value,
     List<string> TaxFor)
        {
            try
            {

                return _Repo.CreateTaxCategory(tax,TaxMasterID,Value,TaxFor);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the tax category.",
                    currentId = 0
                };
            }
        }

        public GenericResponse UpdatetaxCategory(TaxCategoryEntity tax)
        {
            try
            {

                return _Repo.UpdateTaxCategory(tax);
            }
            catch (Exception ex)
            {

                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the tax category.",
                    currentId = 0
                };
            }
        }


        public GenericResponse DeleteTax(int id)
        {
            try
            {

                return _Repo.DeleteTax(id);
            }
            catch (Exception ex)
            {

                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the tax category.",
                    currentId = 0
                };
            }

        }
    }
}
