using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.DTOs;

public class BalanceSheetService : IBalanceSheetService
{
    private readonly IBalanceSheetRepository _balanceSheetRepository;

    public BalanceSheetService(IBalanceSheetRepository balanceSheetRepository)
    {
        _balanceSheetRepository = balanceSheetRepository;
    }

    public IEnumerable<BalanceSheetDTO> GetBalanceSheet()
    {
        return _balanceSheetRepository.GetBalanceSheet();
    }
}
