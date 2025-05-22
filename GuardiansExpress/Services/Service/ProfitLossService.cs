using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Service
{
    public class ProfitLossService : IProfitLossService
    {
        private readonly IFinancialLedgerRepository _ledgerRepo;

        public ProfitLossService(IFinancialLedgerRepository ledgerRepo)
        {
            _ledgerRepo = ledgerRepo;
        }

        public ProfitLossDTO GenerateProfitLossReport(DateTime startDate, DateTime endDate, string branch)
        {
            var transactions = _ledgerRepo.GetTransactionsByDateRange(startDate, endDate, branch);
            var pnl = new ProfitLossDTO();

            //// Group expenses and incomes by description
            //pnl.Expenses = transactions
            //    .Where(t => t.BalanceIn == "Expense")
            //    .Select(g => new ProfitLossItemDTO { Amount = Open })
            //    .ToList();

            //pnl.Incomes = transactions
            //    .Where(t => t.BalanceIn == "Income")
            //    .GroupBy(t => t.Description)
            //    .Select(g => new ProfitLossItemDTO { Description = g.Key, Amount = g.Sum(t => t.Amount) })
            //    .ToList();


            // Calculate totals
            pnl.TotalExpenses = pnl.Expenses.Sum(e => e.Amount);
            pnl.TotalIncomes = pnl.Incomes.Sum(i => i.Amount);
            pnl.NetLossOrProfit = pnl.TotalIncomes - pnl.TotalExpenses;

            return pnl;
        }
    }
}