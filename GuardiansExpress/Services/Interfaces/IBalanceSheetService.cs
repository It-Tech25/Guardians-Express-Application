using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.DTOs;

public interface IBalanceSheetService
{
    IEnumerable<BalanceSheetDTO> GetBalanceSheet();
}
    