using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class GroupSummaryService : IGroupSummaryReportService
    {
        private readonly IGroupSummaryRepository _repository;

        public GroupSummaryService(IGroupSummaryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<LedgerMasterEntity> GetAll(DateTime? startDate, DateTime? endDate, string branch, string accGroup,
            string reportType, string ledger, string agent, string balType, bool withBalance = false, bool showImportant = false)
        {
            return _repository.GetAll();
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
            DateTime? startDate, DateTime? endDate, string branch, string accGroup,
            string reportType, string ledger, string agent, string balType,
            bool withBalance, bool showImportant)
        {
            return _repository.Filter(startDate, endDate, branch, accGroup, reportType, ledger, agent, balType, withBalance, showImportant);
        }
    }
}
