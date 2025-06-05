using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class ProfitAndLossService : IProfitAndLossService
    {
        private readonly IProfitAndLossRepository _repository;

        public ProfitAndLossService(IProfitAndLossRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<LedgerMasterEntity> GetAll(DateTime? startDate, DateTime? endDate, string branch, string accGroup)
        {
            return _repository.GetAll( );
        }

        public LedgerMasterEntity GetById(int id)
        {
            return _repository.GetById(id);
        }

        public bool Create(LedgerMasterEntity entity)
        {
            return _repository.Create(entity);
        }

        public LedgerMasterEntity Update(LedgerMasterEntity entity)
        {
            return _repository.Update(entity);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public IEnumerable<LedgerMasterEntity> Filter(
            DateTime? startDate, DateTime? endDate, string branch, string accGroup)
        {
            return _repository.Filter(startDate, endDate, branch, accGroup);
        }
    }
}
