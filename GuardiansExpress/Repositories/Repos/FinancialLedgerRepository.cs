using GuardiansExpress.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repository
{
    public class FinancialLedgerRepository : IFinancialLedgerRepository
    {
        private readonly MyDbContext _context;

        public FinancialLedgerRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<LedgerMasterEntity> GetTransactionsByDateRange(DateTime startDate, DateTime endDate, string branch)
        {
            return _context.ledgerEntity
                .Where(fl => fl.CreatedOn >= startDate && fl.CreatedOn <= endDate && fl.BranchName == branch)
                .ToList();
        }
    }
}