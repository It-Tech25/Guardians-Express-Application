using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IBankRecoRepository
    {
     
            Task<BankRecoSummaryDTO> GetBankReconciliationDataAsync(string bankName, DateTime onDate);
        }

    }

