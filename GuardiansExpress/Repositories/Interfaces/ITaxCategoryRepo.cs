using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ITaxCategoryRepo
    {
        IEnumerable<TaxCategoryEntity> GetTaxCategory(string searchTerm, int pageNumber, int pageSize);
        TaxCategoryEntity GetTaxCategoryById(int id);
        GenericResponse CreateTaxCategory(TaxCategoryEntity tax,
     List<int> TaxMasterID,
     List<decimal> Value,
     List<string> TaxFor);
        GenericResponse UpdateTaxCategory(TaxCategoryEntity tax);
        GenericResponse DeleteTax(int id);

    }
}
