
using GuardiansExpress.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardiansExpress.Repository
{
    public interface IBillSubmissionRepository
    {
        Task<IEnumerable<BillSubmissionEntity>> GetAllBillsAsync();
        Task<BillSubmissionEntity?> GetBillByIdAsync(int id);
        Task AddBillAsync(BillSubmissionEntity bill);
        Task UpdateBillAsync(BillSubmissionEntity bill);
        Task DeleteBillAsync(int id);
    }
}
