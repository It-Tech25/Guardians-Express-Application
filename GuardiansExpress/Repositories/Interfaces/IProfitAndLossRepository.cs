using System;
using System.Collections.Generic;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Repositories
{
    public interface IProfitAndLossRepository
    {
        IEnumerable<LedgerMasterEntity> GetAll();
        LedgerMasterEntity GetById(int id);
        bool Create(LedgerMasterEntity entity);
        LedgerMasterEntity Update(LedgerMasterEntity entity);
        bool Delete(int id);
        IEnumerable<LedgerMasterEntity> Filter(
            DateTime? startDate, DateTime? endDate, string branch, string accGroup);
    }
}