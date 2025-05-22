using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.DTOs;

public interface ITrialBalanceRepository
{
    List<TrialBalanceDTO> GetTrialBalance();
}
