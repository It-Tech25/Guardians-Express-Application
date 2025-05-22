using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repository
{
    public class ExpCreditNoteRepository : IExpCreditNoteRepository
    {
        private readonly MyDbContext _context;

        public ExpCreditNoteRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<Exp_credit> GetExpCreditNoteDetails(int? branchId, string fromDate, string toDate)
        {
            var query = from ecn in _context.creditnote
                        join b in _context.branch on ecn.Branch equals b.id into branchGroup
                        from branch in branchGroup.DefaultIfEmpty()
                        select new Exp_credit
                        {
                            ExpenceId = ecn.ExpenceId,
                            Branch = ecn.Branch,
                            BranchName = branch != null ? branch.BranchName : string.Empty,
                          InvoiceNo=ecn.InvoiceNo,
                          CostCenter=ecn.CostCenter,
                          Particular=_context.creditledger.Where(a=>a.ExpenceId==ecn.ExpenceId).Select(a=>a.Particular).FirstOrDefault(),
                          Amount= _context.creditledger.Where(a => a.ExpenceId == ecn.ExpenceId).Select(a => a.Amount).FirstOrDefault(),
                          Total= _context.creditledger.Where(a => a.ExpenceId == ecn.ExpenceId).Select(a => a.Total).FirstOrDefault(),
                          ACCDEC= _context.creditledger.Where(a => a.ExpenceId == ecn.ExpenceId).Select(a => a.ACCDEC).FirstOrDefault(),
                          Remarks=ecn.Remarks,
                            NoteDate = ecn.NoteDate,
                            IsActive = ecn.IsActive,
                            IsDeleted = ecn.IsDeleted,
                            CreatedOn = ecn.CreatedOn,
                            CreatedBy = ecn.CreatedBy,
                            UpdatedOn = ecn.UpdatedOn,
                            UpdatedBy = ecn.UpdatedBy
                        };

            // Apply filters
            if (branchId.HasValue && branchId.Value > 0)
            {
                query = query.Where(x => x.Branch == branchId.Value);
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParse(fromDate, out DateTime parsedFromDate))
                {
                    query = query.Where(x => x.NoteDate >= parsedFromDate);
                }
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParse(toDate, out DateTime parsedToDate))
                {
                    query = query.Where(x => x.NoteDate <= parsedToDate);
                }
            }

            // Filter for active records only
            query = query.Where(x => x.IsActive == true && x.IsDeleted == false);

            return query.ToList();
        }
    }
}