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

        // Get all Group Summaries - FIXED: Changed IsDeleted == true to IsDeleted == false
        public IEnumerable<LedgerMasterEntity> GetAll()
        {
            return _context.ledgerEntity
                .Where(g => g.IsDeleted == false) // Fixed: was g.IsDeleted == true
                .OrderBy(g => g.AccGroup)
                .ToList();
        }

        // Get Group Summary by ID
        public LedgerMasterEntity GetById(int id)
        {
            LedgerMasterEntity res = new LedgerMasterEntity();
            try
            {
                res = _context.ledgerEntity
                    .Where(x => x.LedgerId == id && x.IsDeleted == false)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                // Log exception if needed
            }
            return res;
        }

        // Create a new Group Summary
        public bool Create(LedgerMasterEntity entity)
        {
            try
            {
                entity.CreatedOn = DateTime.Now;
                entity.CreatedBy = 1; // Set appropriate user ID
                entity.IsDeleted = false;
                entity.IsActive = true;

                _context.ledgerEntity.Add(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return false;
            }
        }

        // Update an existing Group Summary
        public LedgerMasterEntity Update(LedgerMasterEntity entity)
        {
            var existingEntity = _context.ledgerEntity
                .Where(x => x.LedgerId == entity.LedgerId && x.IsDeleted == false)
                .FirstOrDefault();

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
            var entity = _context.ledgerEntity
                .Where(x => x.LedgerId == id && x.IsDeleted == false)
                .FirstOrDefault();

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;
            entity.UpdatedOn = DateTime.Now;
            entity.UpdatedBy = 1;
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

            return query.OrderBy(g => g.AccGroup).ToList();
        }
    }
}