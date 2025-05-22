using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IExpcredit
    {
        IEnumerable<Exp_credit> GetCreditNotes();
        EXP_CREDITNoteEntity GetCreditNoteById(int id);
        GenericResponse AddCreditNote(EXP_CREDITNoteEntity req);
        GenericResponse DeleteCreditNoteById(int id);
    }
}
