using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories.Repos
{
    public class TrialBalanceRepository : ITrialBalanceRepository
    {
        private readonly MyDbContext _context;

        public TrialBalanceRepository(MyDbContext context)
        {
            _context = context;
        }
        public List<TrialBalanceDTO> GetTrialBalance()
        {
            return _context.ledgerEntity
      .Where(l => l.IsDeleted == false)
      .Select(l => new TrialBalanceDTO
      {
          Branch = l.BranchName,
          AccHead = l.AccHead,
          Group = l.AccGroup,
          SubGroup = _context.SubGroups
              .Where(a => a.subgroupId == l.subgroupId && a.IsDeleted == false)
              .Select(a => a.SubGroupName)
              .FirstOrDefault(),

          // Opening Balance
          OpeningDebit = l.OpeningBalance >= 0 ? l.OpeningBalance : 0,
          OpeningCredit = l.OpeningBalance < 0 ? Math.Abs(l.OpeningBalance) : 0,

          // Current Balance from GR table
          FreightAmount = _context.grDetails
              .Where(gr => gr.Transporter == l.AccHead && gr.IsDeleted == false)
              .Sum(gr => gr.FreightAmount ?? 0),
          HireAmount = _context.grDetails
              .Where(gr => gr.Transporter == l.AccHead && gr.IsDeleted == false)
              .Sum(gr => gr.HireRate ?? 0),

          // Total Balance = Opening + Current
          TotalDebit = (l.OpeningBalance >= 0 ? l.OpeningBalance : 0) +
                      _context.grDetails
                          .Where(gr => gr.Transporter == l.AccHead && gr.IsDeleted == false)
                          .Sum(gr => gr.FreightAmount ?? 0),
          TotalCredit = (l.OpeningBalance < 0 ? Math.Abs(l.OpeningBalance) : 0) +
                       _context.grDetails
                           .Where(gr => gr.Transporter == l.AccHead && gr.IsDeleted == false)
                           .Sum(gr => gr.HireRate ?? 0),

          // For Date filtering - using the latest GR date or ledger creation date
          Date = _context.grDetails
              .Where(gr => gr.Transporter == l.AccHead && gr.IsDeleted == false)
              .OrderByDescending(gr => gr.CreatedOn)
              .Select(gr => gr.CreatedOn)
              .FirstOrDefault() ?? l.CreatedOn
      })
      .ToList();
        }
    }
}