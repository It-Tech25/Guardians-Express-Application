using GuardiansExpress.Models.Entity;

using GuardiansExpress.Repositories.Interfaces;


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardiansExpress.Services.Service
{
    public interface IBillSubmissionService
    {
        Task<IEnumerable<BillSubmissionEntity>> GetAllAsync();
        Task<BillSubmissionEntity?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateBillSubmissionDTO createDto, int userId);
        Task<BillSubmissionEntity?> UpdateAsync(UpdateBillSubmissionDTO updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
