using GuardiansExpress.Models.DTOs;

public interface IBalanceSheetRepository
{
    IEnumerable<BalanceSheetDTO> GetBalanceSheet();
}
