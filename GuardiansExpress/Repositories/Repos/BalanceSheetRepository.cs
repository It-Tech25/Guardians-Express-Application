using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.DTOs; // Assuming your DTO is here

public class BalanceSheetRepository : IBalanceSheetRepository
{
    private readonly MyDbContext _context;

    public BalanceSheetRepository(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<BalanceSheetDTO> GetBalanceSheet()
    {
        return _context.ledgerEntity
            .Select(x => new BalanceSheetDTO
            {
                AccountName = x.AccGroup,
                AccountGroup = x.AccGroup,
                BalanceType = x.AccGroup,
                Balance = x.OpeningBalance,
                
                // Map other fields if needed
            })
            .ToList();
    }
}
