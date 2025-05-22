using GuardiansExpress.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardiansExpress.Services
{
    public interface IDebitNoteService
    {
        IEnumerable<DebitNoteFilterDTO> GetByFilterAsync(DebitNoteFilterDTO filter);
    }
}
