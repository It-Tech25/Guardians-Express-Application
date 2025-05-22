using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IDebitNoteRepository
    {

        IEnumerable<DebitNoteFilterDTO> GetByFilterAsync(DebitNoteFilterDTO filter);

    }
}
