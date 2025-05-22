using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class BankReconciliationController : Controller
    {
        private readonly IBankReconciliationService _bankReconciliationService;
        private readonly MyDbContext _context;

        public BankReconciliationController(IBankReconciliationService bankReconciliationService, MyDbContext context)
        {
            _bankReconciliationService = bankReconciliationService;
            _context = context;
        }
        public IActionResult Index()
        {
            var banks = _context.Banks.ToList(); // Fetch list of bank objects
            return View(banks);
        }


        [HttpGet]
        public async Task<IActionResult> BankReconciliationIndex()
        {
            // Populate bank dropdown
            var bank = _context.ledgerEntity
                .Where(a => a.IsDeleted == false)
                .Select(a => a.BankName)
                .Distinct()
                .ToList();

            ViewBag.bankname = bank;

            // Session check
            var lr = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (lr == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            return View(); // Don't send any data initially
        }
        [HttpGet]
        public IActionResult GetBankReconciliationData(string bankName, string? chequeNo, DateTime? fromDate, DateTime? toDate)
        {
            var vouchers = _context.Vouchers
                .Where(v => v.AccountHead == bankName
                    && (chequeNo == null || v.ChequeNumber == chequeNo)
                    && (!fromDate.HasValue || v.VoucherDate >= fromDate)
                    && (!toDate.HasValue || v.VoucherDate <= toDate))
                .ToList();

            return Json(vouchers);
        }





        public async Task<IActionResult> Details(int id)
        {
            var bankReconciliation = await _bankReconciliationService.GetByIdAsync(id);
            if (bankReconciliation == null)
                return NotFound();
            return View(bankReconciliation);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BankReconciliationDTO model)
        {
            if (ModelState.IsValid)
            {
                await _bankReconciliationService.AddAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Voucher model)
        {
            if (ModelState.IsValid)
            {
                await _bankReconciliationService.UpdateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bankReconciliationService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
