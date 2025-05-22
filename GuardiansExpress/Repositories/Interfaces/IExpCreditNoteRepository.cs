using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repository.Interfaces
{
    public interface IExpCreditNoteRepository
    {
        List<Exp_credit> GetExpCreditNoteDetails(int? branchId, string fromDate, string toDate);
    }
}
