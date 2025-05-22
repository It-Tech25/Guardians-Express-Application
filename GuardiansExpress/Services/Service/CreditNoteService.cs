using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services
{
    public class CreditNoteService : ICreditNoteService
    {
        private readonly ICreditNoteRepo _creditNoteRepo;

        public CreditNoteService(ICreditNoteRepo creditNoteRepo)
        {
            _creditNoteRepo = creditNoteRepo;
        }

        public IEnumerable<CreditNoteModel> GetCreditNotes(string searchTerm, int pageNumber, int pageSize)
        {
            return _creditNoteRepo.GetCreditNotes(searchTerm, pageNumber, pageSize);
        }

        public CreditNoteEntity GetCreditNoteById(int id)
        {
            return _creditNoteRepo.GetCreditNoteById(id);
        }

        public GenericResponse CreateCreditNote(CreditNoteEntity creditNote)
        {
            return _creditNoteRepo.CreateCreditNote(creditNote);
        }

        public GenericResponse UpdateCreditNote(CreditNoteEntity creditNote)
        {
            return _creditNoteRepo.UpdateCreditNote(creditNote);
        }

        public GenericResponse DeleteCreditNote(int id)
        {
            return _creditNoteRepo.DeleteCreditNote(id);
        }
    }
}
