using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Repositories
{
    public class GroupSummaryRepository : IGroupSummaryRepository
    {
        private readonly MyDbContext _context;

        public GroupSummaryRepository(MyDbContext context)
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
            existingEntity.CreatedOn = entity.CreatedOn;
            existingEntity.BranchName = entity.BranchName;
            existingEntity.AccGroup = entity.AccGroup;
            existingEntity.RegistrationType = entity.RegistrationType;
            existingEntity.LedgerId = entity.LedgerId;
            existingEntity.Agent = entity.Agent;
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
            DateTime? startDate, DateTime? endDate, string branch, string accGroup,
            string reportType, string ledger, string agent, string balType,
            bool withBalance, bool showImportant)
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

            if (!string.IsNullOrEmpty(reportType))
                query = query.Where(g => g.RegistrationType.Contains(reportType));

            if (!string.IsNullOrEmpty(agent))
                query = query.Where(g => g.Agent.Contains(agent));

            if (!string.IsNullOrEmpty(balType))
                query = query.Where(g => g.BalanceIn.Contains(balType));

            if (withBalance)
                query = query.Where(g => g.BalanceOpening != 0);

            return query.ToList();
        }
    }
}