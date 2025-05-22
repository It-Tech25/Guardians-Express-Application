using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ITaxMaster
    {
        IEnumerable<TaxMasterDto> GetTaxMastersWithLedger(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateTaxMaster(TaxMasterDto tax);

        GenericResponse UpdateTaxMaster(TaxMasterDto tax);
        GenericResponse DeleteTaxMaster(int id);
    }
}
