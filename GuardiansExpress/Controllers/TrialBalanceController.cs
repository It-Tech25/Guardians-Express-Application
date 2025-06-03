using GuardiansExpress.Models.DTO;
using GuardiansExpress.Models.Entity;
using Microsoft.AspNetCore.Mvc;

public class TrialBalanceController : Controller
{
    private readonly ITrialBalanceService _trialBalanceService;
    private readonly MyDbContext _context;

    public TrialBalanceController(ITrialBalanceService trialBalanceService, MyDbContext context)
    {
        _trialBalanceService = trialBalanceService;
        _context = context;
    }

    public IActionResult TrialBalanceIndex(string Branch, string AccGroup, DateTime? DateFrom, DateTime? DateTo)
    {
        // Fixed the logic error - changed !a.IsDeleted==false to a.IsDeleted==false
        ViewBag.group = _context.ledgerEntity
                            .Where(a => a.IsDeleted == false)
                            .Select(a => a.AccGroup)
                            .Distinct()
                            .ToList();

        var data = _trialBalanceService.GetTrialBalance();

        if (!string.IsNullOrEmpty(Branch))
            data = data.Where(d => d.Branch == Branch).ToList();

        if (!string.IsNullOrEmpty(AccGroup))
            data = data.Where(d => d.Group == AccGroup).ToList();

        if (DateFrom.HasValue)
            data = data.Where(d => d.Date >= DateFrom.Value).ToList();

        if (DateTo.HasValue)
            data = data.Where(d => d.Date <= DateTo.Value).ToList();

        return View(data);
    }
}