using Microsoft.EntityFrameworkCore;
using GuardiansExpress.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Repositories.Interfaces;


namespace GuardiansExpress.Repositories.Repos
{
    public class BillSubmissionRepository : IBillSubmissionRepositorys
    {
        private readonly MyDbContext context;

        public BillSubmissionRepository(MyDbContext _context)
        {
            context = _context;
        }

        // Get all Bill Submissions
        public async Task<IEnumerable<BillSubmissionEntity>> GetAllAsync()
        {
            return await context.BilSubmissions
                .Where(b => b.IsDelete == false) // Fetch only active records
                .ToListAsync();
        }

        // Get Bill Submission by ID
        [HttpGet]
        public async Task<BillSubmissionEntity?> GetByIdAsync(int id)
        {
            BillSubmissionEntity res = new BillSubmissionEntity();
            try
            {
                res = await context.BilSubmissions.FindAsync(id);
            }
            catch (Exception e) { }
            return res;
        }

        // Create a new Bill Submission
        public async Task<bool> CreateAsync(BillSubmissionEntity entity)
        {
            await context.BilSubmissions.AddAsync(entity);
            await context.SaveChangesAsync();
            return true; // Indicating successful creation
        }

        // Update an existing Bill Submission
        public async Task<BillSubmissionEntity?> UpdateAsync(BillSubmissionEntity entity)
        {
            var existingEntity = await context.BilSubmissions.FindAsync(entity.BillSubmissionId);
            if (existingEntity == null) return null;

            // Update fields
            existingEntity.ClientName = entity.ClientName;
            existingEntity.BillNo = entity.BillNo;
            existingEntity.BillSubDate = entity.BillSubDate;
            existingEntity.BillSubmissionBy = entity.BillSubmissionBy;
            existingEntity.ReceivedBy = entity.ReceivedBy;
            existingEntity.HandedOverBy = entity.HandedOverBy;
            existingEntity.DocketNo = entity.DocketNo;
            existingEntity.CourierName = entity.CourierName;
            existingEntity.IsActive = entity.IsActive;
            existingEntity.UpdatedOn = System.DateTime.Now;
            existingEntity.UpdatedBy = 1;

            await context.SaveChangesAsync();
            return existingEntity;
        }

        // Soft Delete a Bill Submission
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await context.BilSubmissions.FindAsync(id);
            if (entity == null)
            {
                return false; // Not found
            }

            entity.IsDelete = true;
            await context.SaveChangesAsync();
            return true; // Successfully deleted
        }
    }
}




