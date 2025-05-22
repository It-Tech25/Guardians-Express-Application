using Azure;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace GuardiansExpress.Controllers
{
    public class LedgerMasterController : Controller

    {
        private readonly ILedgerMasterService _ledgerMasterService;
        private readonly MyDbContext _context;
        public LedgerMasterController(MyDbContext context, ILedgerMasterService tService)
        {

            _context = context;
            _ledgerMasterService = tService;
        }
        public IActionResult LedgerMaster(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            // Pass user details to the view
            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;
            //ViewBag.UserProfileImage = loggedInUser.ProfileImageUrl; // URL to the user's profile image
            ViewBag.subgroup = _context.SubGroups.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
            ViewBag.financialyear = _context.finance.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
            ViewBag.ledger = _context.ledgerEntity.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
            ViewBag.ledgerfinance = _context.financialLedgers.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

            var query = _ledgerMasterService.ledgerEntity(searchTerm, pageNumber, pageSize);

            int totalRecords = query.Count();

            var companies = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Set view data
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(companies);
        }
        public IActionResult LedgerMasterById(int id)
        {
            LedgerMasterDTO obj = new LedgerMasterDTO();
            var res = _ledgerMasterService.LedgerMasterById(id);
            obj = res;
            return View(obj);
        }
        public IActionResult Edit(int id)
        {
            LedgerMasterDTO obj = new LedgerMasterDTO();
            var Ledger = _ledgerMasterService.LedgerMasterById(id);
            obj = Ledger;
            if (Ledger == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult AddorUpdateLedgerMaster(
        LedgerMasterEntity entity,
        List<string> BranchName,
        List<string> NameAddressMobile,
        List<string> Address,
        List<string> CityStatePincode,
        List<string> GSTIN)
        {
            if (entity.LedgerId > 0)
            {

                try
                {
                    var u = _context.ledgerEntity.FirstOrDefault(a => a.LedgerId == entity.LedgerId);
                    var subgroupId = _context.SubGroups
                           .Where(a => a.SubGroupName == entity.AccGroup && a.IsDeleted == false)
                           .Select(a => a.subgroupId)
                           .FirstOrDefault();

                    bool tl, t, ve, ba, wl;
                    if (u.TaxLedger == null || entity.TaxLedger == null)
                    {
                        tl = false;
                    }

                    else { tl = true; }
                    if (u.Taxable == null || entity.Taxable == null)
                    {
                        t = false;
                    }
                    else { t = true; }
                    if (u.BankAccount == null || entity.BankAccount == null)
                    {
                        ba = false;
                    }
                    else { ba = true; }
                    if (u.VehicleExpense == null || entity.VehicleExpense == null)
                    {
                        ve = false;
                    }
                    else { ve = true; }
                    if (u.WalkinLedger == null || entity.WalkinLedger == null)
                    {
                        wl = false;
                    }
                    else { wl = true; }

                    entity.TaxLedger = tl;
                    entity.BankAccount = ba;
                    entity.Taxable = t;
                    entity.WalkinLedger = wl;
                    entity.VehicleExpense = ve;
                    entity.IsActive = true;
                    entity.IsDeleted = false;

                    entity.UpdatedOn = DateTime.Now;
                    entity.UpdatedBy = u.LedgerId;
                    entity.subgroupId = subgroupId;

                    _context.Entry(u).CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                    List<AddressDetailsEntity> addressDetails = new List<AddressDetailsEntity>();

                    // Ensure all lists have the same count
                    int count = BranchName.Count;

                    for (int i = 0; i < count; i++)
                    {
                        if (!string.IsNullOrEmpty(BranchName[i])) // Ensure no empty values
                        {
                            addressDetails.Add(new AddressDetailsEntity
                            {
                                LedgerId = entity.LedgerId,
                                BranchName = BranchName[i],
                                NameAddressMobile = NameAddressMobile[i],
                                Address = Address[i],
                                CityStatePincode = CityStatePincode[i],
                                GSTIN = GSTIN[i]
                            });
                        }
                    }
                    var existingAddresses = _context.address.Where(a => a.LedgerId == entity.LedgerId).ToList();
                    _context.address.RemoveRange(existingAddresses);

                    // Add new/updated addresses
                    _context.address.AddRange(addressDetails);

                    _context.SaveChanges();

                    if (addressDetails.Count > 0)
                    {
                        var response = _ledgerMasterService.UpdateLedgerMaster(entity, addressDetails);
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {

                int count = _context.ledgerEntity.Where(a => a.AccHead == entity.AccHead && a.IsDeleted == false).Count();
                if (count < 1)
                {
                    try
                    {
                        var subgroupId = _context.SubGroups
                           .Where(a => a.SubGroupName == entity.AccGroup && a.IsDeleted == false)
                           .Select(a => a.subgroupId)
                           .FirstOrDefault();
                        bool tl, t, ve, ba, wl;
                        if (entity.TaxLedger == null)
                        {
                            tl = false;
                        }
                        else { tl = true; }
                        if (entity.Taxable == null)
                        {
                            t = false;
                        }
                        else { t = true; }
                        if (entity.BankAccount == null)
                        {
                            ba = false;
                        }
                        else { ba = true; }
                        if (entity.VehicleExpense == null)
                        {
                            ve = false;
                        }
                        else { ve = true; }
                        if (entity.WalkinLedger == null)
                        {
                            wl = false;
                        }
                        else { wl = true; }
                        entity.BankAccount = ba;
                        entity.Taxable = t;
                        entity.TaxLedger = tl;
                        entity.VehicleExpense = ve;
                        entity.subgroupId = subgroupId;
                        entity.IsDeleted = false;
                        entity.IsActive = true;
                        entity.WalkinLedger = wl;
                        entity.CreatedOn = DateTime.Today;
                        entity.CreatedBy = entity.LedgerId;
                        _context.ledgerEntity.Add(entity);
                        _context.SaveChanges();
                        int newLedgerId = entity.LedgerId;

                        List<AddressDetailsEntity> Entry = new List<AddressDetailsEntity>();


                        for (int i = 0; i < BranchName.Count; i++)
                        {
                            AddressDetailsEntity address = new AddressDetailsEntity
                            {
                                LedgerId = newLedgerId,
                                BranchName = BranchName[i],
                                NameAddressMobile = NameAddressMobile[i],
                                Address = Address[i],
                                CityStatePincode = CityStatePincode[i],
                                GSTIN = GSTIN[i]
                            };
                            Entry.Add(address);
                        }

                        _context.address.AddRange(Entry);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return RedirectToAction("LedgerMaster");
        }
        [HttpPost]
        public IActionResult DeleteLedgerMaster([FromBody] LedgerMasterEntity model)
        {
            if (model == null || model.LedgerId <= 0)
            {
                return Json(new { success = false, message = "Invalid ID" });
            }


            var res = _ledgerMasterService.DeleteLedgerMaster(model.LedgerId);

            if (res.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = res.message });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFinancialLedger(List<FinancialLedgerEntity> FinancialLedgers, string AccountHead)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ledgerId = _context.ledgerEntity
                        .Where(a => a.AccHead == AccountHead && a.IsDeleted == false)
                        .Select(a => a.LedgerId)
                        .FirstOrDefault();

                    if (ledgerId == 0)
                    {
                        // Handle case where LedgerId is not found
                        return RedirectToAction("LedgerMaster");
                    }

                    foreach (var entity in FinancialLedgers)
                    {
                        var existingRecord = _context.financialLedgers
                            .FirstOrDefault(f => f.LedgerId == ledgerId && f.FinancialYear == entity.FinancialYear);

                        if (existingRecord != null)
                        {
                            // Update existing record
                            existingRecord.OpeningBalance = entity.OpeningBalance;
                            existingRecord.BalanceIn = entity.BalanceIn;
                            existingRecord.IsActive = true; // Ensuring it's active
                            existingRecord.IsDeleted = false;
                        }
                        else
                        {
                            // Insert new record if not found
                            FinancialLedgerEntity newEntry = new FinancialLedgerEntity
                            {
                                LedgerId = ledgerId,
                                AccountHead = AccountHead,
                                FinancialYear = entity.FinancialYear,
                                OpeningBalance = entity.OpeningBalance,
                                BalanceIn = entity.BalanceIn,
                                IsDeleted = false,
                                IsActive = true,
                            };
                            _context.financialLedgers.Add(newEntry);
                        }
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return RedirectToAction("LedgerMaster");
            }
            return RedirectToAction("LedgerMaster");
        }

    }
}




