using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IFinanceService
    {
        IEnumerable<FinancialyearDto> Get();
        GenericResponse Add(FinancialyearDto fy, string serializedInvoiceData, string serializedBillData);
        Task<bool> UpdateFinancialYearAsync(FinancialyearDto financialYear, string serializedInvoiceData, string serializedBillData);
        Task<bool> DeleteFinancialYearAsync(int id);
        FinancialyearEntity GetFinancialYearById(int id);
        List<FinancialInvoiceEntity> GetFinancialInvoiceById(int id);
        List<FinancialBillEntity> GetFinancialBillById(int id);
    }
}
