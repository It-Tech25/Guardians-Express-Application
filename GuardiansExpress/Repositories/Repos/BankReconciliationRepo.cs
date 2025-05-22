using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuardiansExpress.Repositories
{
    public class BankReconciliationRepository : IBankReconciliationRepository
    {
        private readonly MyDbContext _context;

        public BankReconciliationRepository(MyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Voucher>> GetAllAsync()
        {
            var res = await _context.Vouchers
                .Where(a => a.IsDelete==false && a.IsActive==true )
                .ToListAsync();

            return res;
        }


        public async Task<BankReconciliationDTO> GetByIdAsync(int id)
        {
            var entity = await _context.Banks.FindAsync(id);
            if (entity == null) return null;

            return new BankReconciliationDTO
            {
                BankId = entity.BankId,
                BankName = entity.BankName,
                DateFrom = entity.DateFrom,
                DateTo = entity.DateTo,
                DocNo = entity.DocNo,
                BankDate = entity.BankDate,
                ChequeNo = entity.ChequeNo,
                AcDescription = entity.AcDescription,
                RefDescription = entity.RefDescription,
                Amount = entity.Amount,
                Type = entity.Type,
                ReconcileData = entity.ReconcileData,
                IsDelete = entity.IsDelete,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            };
        }

        public async Task AddAsync(BankReconciliationDTO bankReconciliation)
        {
            if (bankReconciliation == null) throw new ArgumentNullException(nameof(bankReconciliation));

            var entity = new BankReconciliationEntity
            {
                BankId = bankReconciliation.BankId,
                BankName = bankReconciliation.BankName,
                DateFrom = bankReconciliation.DateFrom,
                DateTo = bankReconciliation.DateTo,
                DocNo = bankReconciliation.DocNo,
                BankDate = bankReconciliation.BankDate,
                ChequeNo = bankReconciliation.ChequeNo,
                AcDescription = bankReconciliation.AcDescription,
                RefDescription = bankReconciliation.RefDescription,
                Amount = bankReconciliation.Amount,
                Type = bankReconciliation.Type,
                ReconcileData = bankReconciliation.ReconcileData,
                IsDelete = bankReconciliation.IsDelete,
                CreatedBy = bankReconciliation.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = bankReconciliation.UpdatedBy,
                UpdatedOn = DateTime.UtcNow
            };

            _context.Banks.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Voucher bankReconciliation)
        {
            var entity = await _context.Vouchers.FindAsync(bankReconciliation.ChequeNumber);
            if (entity == null) throw new KeyNotFoundException("Bank reconciliation record not found.");

           
            entity.ReconcileDate = bankReconciliation.ReconcileDate;

            _context.Vouchers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Banks.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("Bank reconciliation record not found.");

            entity.IsDelete = true; // Soft delete
            _context.Banks.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
