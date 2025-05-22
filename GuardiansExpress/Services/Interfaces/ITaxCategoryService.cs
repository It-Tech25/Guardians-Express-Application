using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface ITaxCategoryService
    {
        IEnumerable<TaxCategoryEntity> GetTaxCategory(string searchTerm, int pageNumber, int pageSize);
        TaxCategoryEntity GetTaxCategoryById(int id);
        GenericResponse CreateTaxCategory(TaxCategoryEntity tax,
     List<int> TaxMasterID,
     List<decimal> Value,
     List<string> TaxFor);
        GenericResponse UpdatetaxCategory(TaxCategoryEntity tax);
        GenericResponse DeleteTax(int id);
    }
}
