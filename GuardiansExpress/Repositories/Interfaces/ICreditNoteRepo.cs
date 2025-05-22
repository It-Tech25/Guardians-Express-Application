using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ICreditNoteRepo
    {
        IEnumerable<CreditNoteModel> GetCreditNotes(string searchTerm, int pageNumber, int pageSize);
        CreditNoteEntity GetCreditNoteById(int id);
        GenericResponse CreateCreditNote(CreditNoteEntity creditNote);
        GenericResponse UpdateCreditNote(CreditNoteEntity creditNote);
        GenericResponse DeleteCreditNote(int id);
    }
}
