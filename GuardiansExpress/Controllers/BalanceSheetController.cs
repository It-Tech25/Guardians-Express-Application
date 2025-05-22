using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.DTO;

public class BalanceSheetController : Controller
{
    private readonly IBalanceSheetService _balanceSheetService;

    public BalanceSheetController(IBalanceSheetService balanceSheetService)
    {
        _balanceSheetService = balanceSheetService;
    }

    public IActionResult BalanceSheetIndex()
    {
        var balanceSheets = _balanceSheetService.GetBalanceSheet();
        return View(balanceSheets); // You should have a corresponding Index.cshtml
    }
    
}
