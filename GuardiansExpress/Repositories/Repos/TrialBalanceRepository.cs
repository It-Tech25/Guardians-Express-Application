using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;

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
                .Where(l => l.IsDeleted==false)
                .Select(l => new TrialBalanceDTO
                {
                    Branch = l.BranchName,
                    AccHead = l.AccHead,
                    Group = l.AccGroup,
                    SubGroup = _context.SubGroups.Where(a=>a.subgroupId==l.subgroupId && a.IsDeleted==false).Select(a=>a.SubGroupName).FirstOrDefault(),

                    // Opening Balance
                    OpeningDebit = l.OpeningBalance >= 0 ? l.OpeningBalance : 0,
                    OpeningCredit = l.OpeningBalance < 0 ? Math.Abs(l.OpeningBalance) : 0,

                    //// Current Movement
                    //CurrentDebit = l.BalanceIn != null ? l.BalanceIn : 0,
                    //CurrentCredit = l.BalanceIn  0 ? Math.Abs(l.BalanceIn) : 0,

                    //// Total Balance = Opening + Current
                    //TotalDebit = (l.OpeningBalance + l.BalanceIn) >= 0 ? (l.OpeningBalance + l.BalanceIn) : 0,
                    //TotalCredit = (l.OpeningBalance + l.BalanceIn) < 0 ? Math.Abs(l.OpeningBalance + l.BalanceIn) : 0,

                    //// For Date filtering
                    //Date = l.TransactionDate
                })
                .ToList();
        }
    }
}

