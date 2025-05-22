using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IBillSubmissionRepositorys
    {
        Task<IEnumerable<BillSubmissionEntity>> GetAllAsync();
        Task<BillSubmissionEntity?> GetByIdAsync(int id);
        Task<bool> CreateAsync(BillSubmissionEntity entity);// ✅ Fixed method name
        Task<BillSubmissionEntity?> UpdateAsync(BillSubmissionEntity entity);
        Task<bool> DeleteAsync(int id); // ✅ Fixed return type
    }
}
