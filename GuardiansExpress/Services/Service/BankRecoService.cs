using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Services
{
    public class BankRecoService : IBankRecoService
    {
        private readonly MyDbContext _context;

        public BankRecoService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetBankNamesAsync()
        {
            return await _context.ledgerEntity
                .Where(x => x.AccGroup == "Bank") // Filter for bank accounts
                .Select(x => x.AccHead)
                .Distinct()
                .ToListAsync();
        }

        public async Task<BankRecoSummaryDTO> GetReportAsync(string bankName, DateTime onDate)
        {
            var vouchers = await _context.Vouchers
                .Where(v => v.AccountHead == bankName && v.IsDelete==false)
                .ToListAsync();

            if (!vouchers.Any())
                return null;

            var voucherIds = vouchers.Select(v => v.VoucherId).ToList();
            var voucherNumbers = vouchers.Select(v => v.VoucherNumber).Distinct().ToList();

            var voucherDetails = await _context.voucherDetails
                .Where(d => voucherIds.Contains(d.VoucherId))
                .ToListAsync();

            var ledgerEntries = await _context.ledgerEntity
                .Where(l => l.IsDeleted==false && l.BankName == bankName)
                .ToListAsync();

            // Balance As Per Books calculation from VoucherDetails
            decimal balanceAsPerBooks = 0;
            foreach (var detail in voucherDetails)
            {
                if (detail.TransactionType == "DR")
                    balanceAsPerBooks += detail.Amount ?? 0;
                else if (detail.TransactionType == "CR")
                    balanceAsPerBooks -= detail.Amount ?? 0;
            }

            // Deposited and Issued But Not Cleared
            // Get vouchers with ReconciliationDate not null
            var reconciledVouchers = await _context.Vouchers
                .Where(v => v.ReconcileDate != null)
                .Select(v => new { v.VoucherId, v.AccountHead })
                .ToListAsync();

            var reconciledVoucherIds = reconciledVouchers.Select(v => v.VoucherId).ToList();

            // Get VoucherDetails related to those vouchers
            var voucherDetailss = await _context.voucherDetails
                .Where(d => reconciledVoucherIds.Contains(d.VoucherId))
                .ToListAsync();

            // Now join with ledgerEntity based on AccountHead = BankName
            var ledgerEntriess = await _context.ledgerEntity
                .Where(l => l.IsDeleted==false)
                .ToListAsync();

            var depositedButNotCleared = (from d in voucherDetails
                                          join v in reconciledVouchers on d.VoucherId equals v.VoucherId
                                          join l in ledgerEntries on v.AccountHead equals l.BankName
                                          where d.TransactionType == "Deposit"
                                          select d.Amount).Sum();

            var issuedButNotCleared = (from d in voucherDetails
                                       join v in reconciledVouchers on d.VoucherId equals v.VoucherId
                                       join l in ledgerEntries on v.AccountHead equals l.BankName
                                       where d.TransactionType == "Issue"
                                       select d.Amount).Sum();

            decimal balanceAsPerBank = balanceAsPerBooks - depositedButNotCleared + issuedButNotCleared ??0;

            return new BankRecoSummaryDTO
            {
                BankName = bankName,
                OnDate = onDate,
                BalanceAsPerBooks = balanceAsPerBooks,
                DepositedButNotCleared = depositedButNotCleared??0,
                IssuedButNotCleared = issuedButNotCleared??0,
                BalanceAsPerBank = balanceAsPerBank
            };
        }

    }
}
