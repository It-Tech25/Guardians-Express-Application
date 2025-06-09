using GuardiansExpress.Models.Entity;
using Microsoft.VisualBasic;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public interface IBalanceSheetService
    {
        IEnumerable<LedgerMasterEntity> GetAll(DateTime? startDate, DateTime? endDate, string branch, string accGroup);
        LedgerMasterEntity GetById(int id);
        bool Create(LedgerMasterEntity entity);
        LedgerMasterEntity Update(LedgerMasterEntity entity);
        bool Delete(int id);
        IEnumerable<LedgerMasterEntity> Filter(
            DateTime? startDate, DateTime? endDate, string branch, string accGroup);
    }
}
