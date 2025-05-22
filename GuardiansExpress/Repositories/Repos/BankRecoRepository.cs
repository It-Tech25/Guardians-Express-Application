
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GuardiansExpress.Repositories.Implementations
{
    public class BankRecoRepository : IBankRecoRepository
    {
        private readonly MyDbContext _context;

        public BankRecoRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<BankRecoSummaryDTO> GetBankReconciliationDataAsync(string bankName, DateTime onDate)
        {
            var ledger = await _context.ledgerEntity    
                .FirstOrDefaultAsync(x => x.AccHead == bankName);

            if (ledger == null)
            {
                return new BankRecoSummaryDTO
                {
                    BankName = bankName,
                    OnDate = onDate,
                    BalanceAsPerBooks = 0,
                    DepositedButNotCleared = 0,
                    IssuedButNotCleared = 0,
                    BalanceAsPerBank = 0
                };
            }

            decimal balanceBooks = ledger.OpeningBalance;

            // Simulate additional fields for demo purposes
            decimal depositedButNotCleared = 1000; // Replace with real logic
            decimal issuedButNotCleared = 500;     // Replace with real logic

            decimal balanceAsPerBank = balanceBooks - depositedButNotCleared + issuedButNotCleared;

            return new BankRecoSummaryDTO
            {
                BankName = ledger.AccHead,
                OnDate = onDate,
                BalanceAsPerBooks = balanceBooks,
                DepositedButNotCleared = depositedButNotCleared,
                IssuedButNotCleared = issuedButNotCleared,
                BalanceAsPerBank = balanceAsPerBank
            };
        }
    }
}
