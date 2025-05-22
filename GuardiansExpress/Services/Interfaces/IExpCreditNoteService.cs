using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IExpCreditNoteService
    {
        List<Exp_credit> GetExpCreditNoteDetails(int? branchId, string fromDate, string toDate);
    }
}