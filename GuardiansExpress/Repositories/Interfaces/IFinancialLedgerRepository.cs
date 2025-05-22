using GuardiansExpress.Models.Entity;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Repository
{
    public interface IFinancialLedgerRepository
    {
        List<LedgerMasterEntity> GetTransactionsByDateRange(DateTime startDate, DateTime endDate, string branch);
    }
}