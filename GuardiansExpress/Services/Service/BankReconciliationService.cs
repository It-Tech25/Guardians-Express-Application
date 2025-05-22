using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardiansExpress.Services
{
    public class BankReconciliationService : IBankReconciliationService
    {
        private readonly IBankReconciliationRepository _repository;

        public BankReconciliationService(IBankReconciliationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Voucher>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BankReconciliationDTO> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(BankReconciliationDTO bankReconciliation)
        {
            if (bankReconciliation == null)
                throw new ArgumentNullException(nameof(bankReconciliation));

            await _repository.AddAsync(bankReconciliation);
        }

        public async Task UpdateAsync(Voucher bankReconciliation)
        {
            if (bankReconciliation == null)
                throw new ArgumentNullException(nameof(bankReconciliation));

            await _repository.UpdateAsync(bankReconciliation);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
