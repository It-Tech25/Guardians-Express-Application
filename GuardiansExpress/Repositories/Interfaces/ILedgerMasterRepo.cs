using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ILedgerMasterRepo
    {
        IEnumerable<LedgerMasterDTO> ledgerEntity(string searchTerm, int pageNumber, int pageSize);
        LedgerMasterDTO LedgerMasterById(int id);
        GenericResponse CreateLedgerMaster(LedgerMasterEntity res);
        GenericResponse UpdateLedgerMaster(LedgerMasterEntity req, List<AddressDetailsEntity> updatedAddresses);
        GenericResponse DeleteLedgerMaster(int id);
    }
}
