using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class LedgerMasterService : ILedgerMasterService

    {

        private readonly ILedgerMasterRepo repo;
        private bool _disposed;

        public LedgerMasterService(ILedgerMasterRepo _repo)
        {
            repo = _repo;

        }
        public IEnumerable<LedgerMasterDTO> ledgerEntity(string searchTerm, int pageNumber, int pageSize)
        {
            return repo.ledgerEntity(searchTerm, pageNumber, pageSize);
        }
        public LedgerMasterDTO LedgerMasterById(int id)
        {
            return repo.LedgerMasterById(id);
        }
        public GenericResponse CreateLedgerMaster(LedgerMasterEntity res)
        {
            return repo.CreateLedgerMaster(res);
        }
        public GenericResponse UpdateLedgerMaster(LedgerMasterEntity req, List<AddressDetailsEntity> updatedAddresses)
        {
            return repo.UpdateLedgerMaster(req, updatedAddresses);
        }
        public GenericResponse DeleteLedgerMaster(int id)
        {
            return repo.DeleteLedgerMaster(id);
        }
    }
}


