using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class FinancialService: IFinanceService
    {
        private readonly IFinancialRepo _fRepo;

        // Constructor
        public FinancialService(IFinancialRepo fRepo)
        {
            _fRepo = fRepo;
        }
        public IEnumerable<FinancialyearDto> Get()
        {
            return _fRepo.Get();
        }
        public GenericResponse Add(FinancialyearDto fy, string serializedInvoiceData, string serializedBillData)
        {
            return _fRepo.Add(fy,serializedInvoiceData,serializedBillData);
        }
        public async Task<bool> UpdateFinancialYearAsync(FinancialyearDto financialYearDto, string serializedInvoiceData, string serializedBillData)
        {
           
            return await _fRepo.UpdateFinancialYearAsync(financialYearDto, serializedInvoiceData, serializedBillData);
        }

        public async Task<bool> DeleteFinancialYearAsync(int id)
        {

            return await _fRepo.DeleteFinancialYearAsync(id);
        }
        public FinancialyearEntity GetFinancialYearById(int id)
        {
            return _fRepo.GetFinancialYearById(id);
        }
       public List<FinancialInvoiceEntity> GetFinancialInvoiceById(int id)
        {
            return _fRepo.GetFinancialInvoiceById(id);
        }
        public List<FinancialBillEntity> GetFinancialBillById(int id)
        {
            return _fRepo.GetFinancialBillById(id);
        }
    }
}
