using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IBankRecoService
    {
        Task<BankRecoSummaryDTO> GetReportAsync(string bankName, DateTime onDate);
        Task<IEnumerable<string>> GetBankNamesAsync();
    }

}
