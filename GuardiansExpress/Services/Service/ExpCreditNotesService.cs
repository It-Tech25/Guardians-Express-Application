using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services
{
    public class ExpCreditNotesService : IExpCreditNoteService
    {
        private readonly IExpCreditNoteRepository _expCreditNoteRepository;

        public ExpCreditNotesService(IExpCreditNoteRepository expCreditNoteRepository)
        {
            _expCreditNoteRepository = expCreditNoteRepository;
        }

        public List<Exp_credit> GetExpCreditNoteDetails(int? branchId, string fromDate, string toDate)
        {
            return _expCreditNoteRepository.GetExpCreditNoteDetails(branchId, fromDate, toDate);
        }
    }
}