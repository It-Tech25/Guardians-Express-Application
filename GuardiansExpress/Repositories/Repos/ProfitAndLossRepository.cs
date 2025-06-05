using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Repositories
{
    public class ProfitAndLossRepository : IProfitAndLossRepository
    {
        private readonly MyDbContext _context;

        public ProfitAndLossRepository(MyDbContext context)
        {
            _context = context;
        }

        // Get all Group Summaries
        public IEnumerable<LedgerMasterEntity> GetAll()
        {
            return _context.ledgerEntity
                .Where(g => g.IsDeleted == true)
                .ToList();
        }

        // Get Group Summary by ID
        public LedgerMasterEntity GetById(int id)
        {
            LedgerMasterEntity res = new LedgerMasterEntity();
            try
            {
                res = _context.ledgerEntity.Find(id);
            }
            catch (Exception e) { }
            return res;
        }

        // Create a new Group Summary
        public bool Create(LedgerMasterEntity entity)
        {
            _context.ledgerEntity.Add(entity);
            _context.SaveChanges();
            return true;
        }

        // Update an existing Group Summary
        public LedgerMasterEntity Update(LedgerMasterEntity entity)
        {
            var existingEntity = _context.ledgerEntity.Find(entity.LedgerId);
            if (existingEntity == null) return null;

            // Update fields
           
            existingEntity.AccGroup = entity.AccGroup;
            existingEntity.BalanceIn = entity.BalanceIn;
            existingEntity.BalanceOpening = entity.BalanceOpening;
            existingEntity.IsActive = entity.IsActive;
            existingEntity.UpdatedOn = DateTime.Now;
            existingEntity.UpdatedBy = 1;

            _context.SaveChanges();
            return existingEntity;
        }

        // Soft Delete a Group Summary
        public bool Delete(int id)
        {
            var entity = _context.ledgerEntity.Find(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }

        // Filter group summaries based on criteria
        public IEnumerable<LedgerMasterEntity> Filter(
            DateTime? startDate, DateTime? endDate, string branch, string accGroup)
        {
            var query = _context.ledgerEntity.Where(g => g.IsDeleted == false);

            if (startDate.HasValue)
                query = query.Where(g => g.CreatedOn >= startDate);

            if (endDate.HasValue)
                query = query.Where(g => g.CreatedOn <= endDate);

            if (!string.IsNullOrEmpty(branch))
                query = query.Where(g => g.BranchName.Contains(branch));

            if (!string.IsNullOrEmpty(accGroup))
                query = query.Where(g => g.AccGroup.Contains(accGroup));

            return query.ToList();
        }
    }
}