using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface ITaxMasterService
    {
        IEnumerable<TaxMasterDto> GetTaxMastersWithLedger(string searchTerm, int pageNumber, int pageSize);
        GenericResponse CreateTaxMaster(TaxMasterDto tax);

        GenericResponse UpdateTaxMaster(TaxMasterDto tax);
        GenericResponse DeleteTaxMaster(int id);
    }
}
