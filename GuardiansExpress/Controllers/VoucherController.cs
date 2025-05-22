using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace GuardiansExpress.Controllers
{
    public class VoucherController : Controller
    {
        private readonly IVoucherService _voucherService;
        private readonly MyDbContext _context;
        private const int DefaultPageSize = 10;

        public VoucherController(IVoucherService voucherService, MyDbContext context)
        {
            _voucherService = voucherService;
            _context = context;
        }

        public async Task<IActionResult> VoucherIndex(string voucherType = null, int page = 1, int pageSize = DefaultPageSize)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            // Get ledger accounts for dropdowns
            ViewBag.LedgerAccounts = _context.ledgerEntity
                .Where(l => l.IsDeleted == false)
                .Select(a => a.AccHead)
                .Distinct()
                .ToList() ?? new List<string>();

            ViewBag.banks = _context.ledgerEntity
                .Where(l => l.IsDeleted == false && l.AccGroup=="Bank Accounts")
                .Select(l => l.AccHead)
                .ToList();

            // Get all vouchers
            var allVouchers = _voucherService.GetAllVouchers().ToList(); // Ensure materialized list

            // Apply filter if specified
            if (!string.IsNullOrEmpty(voucherType))
            {
                allVouchers = allVouchers.Where(v => v.VoucherType == voucherType).ToList();
            }

            // Calculate pagination values
            int totalVouchers = allVouchers.Count; // Using Count property instead of Count()
            int totalPages = (int)Math.Ceiling(totalVouchers / (double)pageSize);

            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, totalPages));

            // Apply pagination
            var paginatedVouchers = allVouchers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Pass data to view
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalVouchers = totalVouchers;
            ViewBag.VoucherType = voucherType;

            return View(paginatedVouchers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Voucher voucher, string serializedvoucherData, int accHeadId)
        {
            try
            {
                if (accHeadId > 0)
                {
                    var accountHead = _context.ledgerEntity.FirstOrDefault(l => l.LedgerId == accHeadId && l.IsDeleted == false);
                    if (accountHead == null)
                    {
                        TempData["ErrorMessage"] = "Invalid account selected";
                        return RedirectToAction("VoucherIndex");
                    }
                    voucher.AccountHead = accountHead.AccHead;
                }

                _voucherService.AddVoucherAsync(voucher, serializedvoucherData);
                TempData["SuccessMessage"] = "Voucher created successfully";
                return RedirectToAction("VoucherIndex");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating voucher: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while saving the voucher";
                return RedirectToAction("VoucherIndex");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContraCreate(Voucher voucher, int accHeadId)
        {
            try
            {
                if (accHeadId > 0)
                {
                    var accountHead = _context.ledgerEntity.FirstOrDefault(l => l.LedgerId == accHeadId && l.IsDeleted == false);
                    if (accountHead == null)
                    {
                        TempData["ErrorMessage"] = "Invalid account selected";
                        return RedirectToAction("VoucherIndex");
                    }
                    voucher.AccountHead = accountHead.AccHead;
                }

                _voucherService.AddContraVoucherAsync(voucher);
                TempData["SuccessMessage"] = "Voucher created successfully";
                return RedirectToAction("VoucherIndex");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating voucher: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while saving the voucher";
                return RedirectToAction("VoucherIndex");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Voucher voucher, int accHeadId)
        {
            if (id != voucher.VoucherId)
            {
                return NotFound();
            }

            try
            {
                if (accHeadId > 0)
                {
                    var accountHead = _context.ledgerEntity.FirstOrDefault(l => l.LedgerId == accHeadId);
                    voucher.AccountHead = accountHead?.AccHead;
                }

                _voucherService.UpdateVoucherAsync(voucher);
                TempData["SuccessMessage"] = "Voucher updated successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating voucher: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while updating the voucher";
            }

            return RedirectToAction("VoucherIndex");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _voucherService.DeleteVoucherAsync(id);
                TempData["SuccessMessage"] = "Voucher deleted successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting voucher: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the voucher";
            }

            return RedirectToAction("VoucherIndex");
        }
        [HttpGet]
        public IActionResult GetBankBalance(string bankName)
        {
            try
            {
                // Replace this with your actual logic to get the bank balance
                decimal balance = GetCurrentBankBalanceFromDatabase(bankName);

                return Json(new { success = true, balance = balance });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public decimal GetCurrentBankBalanceFromDatabase(string bankName)
        {
            decimal amount = _context.ledgerEntity
                .Where(a => a.AccHead == bankName && a.AccGroup == "Bank Accounts")
                .Select(a => a.BalanceOpening ?? 0)
                .FirstOrDefault();

            return amount;
        }

    }
}