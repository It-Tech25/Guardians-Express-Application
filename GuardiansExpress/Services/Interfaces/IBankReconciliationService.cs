using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IBankReconciliationService
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<BankReconciliationDTO> GetByIdAsync(int id);
        Task AddAsync(BankReconciliationDTO bankReconciliation);
        Task UpdateAsync(Voucher bankReconciliation);
        Task DeleteAsync(int id);
    }
}
