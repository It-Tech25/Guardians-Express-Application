using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GuardiansExpress.Controllers
{
    public class BillAdjustmentController : Controller
    {
        private readonly IBillAdjustmentService _service;
        private readonly ILedgerMasterService _lService;
        private readonly IVoucherService _vService;
        private readonly MyDbContext _context;
        public BillAdjustmentController(IBillAdjustmentService service, ILedgerMasterService lService, IVoucherService vService, MyDbContext context)
        {
            _service = service;
            _context = context;
            _lService=lService;
            _vService = vService;
        }

        public IActionResult BillAdjustmentIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            LoginResponse lr = new LoginResponse();
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
            var query = _context.BillAdjustments.AsQueryable();


            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var BillAdjustmentEntities = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var BillAdjustmentDTOs = BillAdjustmentEntities.Select(b => new BillAdjustmentDTO
            {
                BalanceId = b.BalanceId,
                BalanceBills = b.BalanceBills,

                UnderGroup = b.UnderGroup,
                Party = b.Party,
                VoucherNumber = b.VoucherNumber,
                VoucherDate = b.VoucherDate,
                BillNumber= b.BillNumber,
                Bill_Date= b.Bill_Date,
                BillAmt = b.BillAmt,
                BalAmt = b.BalAmt,
                RefDescription = b.RefDescription,
                Particular = b.Particular,
                IsActive = b.IsActive,
                IsDeleted = b.IsDeleted,
                CreatedOn = b.CreatedOn,
                CreatedBy = b.CreatedBy,
                UpdatedOn = b.UpdatedOn,
                UpdatedBy = b.UpdatedBy
            }).ToList();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;

            var l = _context.ledgerEntity.Where(a => a.IsDeleted == false).Select(a => a.AccHead).Distinct().ToList();
            var v = (from voucher in _context.Vouchers
                     join details in _context.voucherDetails
                     on voucher.VoucherId equals details.VoucherId
                     where voucher.IsDelete == false
                     && details.BalAmount > 0
                     select voucher.VoucherNumber)
         .Distinct()
         .ToList();

            ViewBag.l = l;
            ViewBag.v = v;

            return View(BillAdjustmentDTOs);
        }

        [HttpPost]
        public ActionResult Save(BillAdjustmentDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var billAdjustment = new BillAdjustmentEntity
                    {
                        BalanceId = model.BalanceId,
                        BalanceBills = model.BalanceBills,
                        UnderGroup = model.UnderGroup,
                        Party = model.Party,
                        VoucherNumber = model.VoucherNumber,
                        VoucherDate = model.VoucherDate,
                        BillNumber= model.BillNumber,
                        Bill_Date= model.Bill_Date,
                        BillAmt = model.BillAmt,
                        BalAmt = model.BalAmt,
                        RefDescription = model.RefDescription,
                        Particular = model.Particular,
                        IsActive = model.IsActive ?? true,
                        IsDeleted = model.IsDeleted ?? false,
                        CreatedOn = DateTime.Now,
                        CreatedBy = model.CreatedBy
                    };

                    _context.BillAdjustments.Add(billAdjustment);
                    _context.SaveChanges(); // Save to get ID

                   
                    if (model.BillItems != null && model.BillItems.Any())
                    {
                        foreach (var item in model.BillItems)
                        {
                            var billItem = new BillAdjustmentDetailsEntity
                            {
                                BalanceFID = billAdjustment.BalanceId, 
                                RefNo = item.RefNo,
                                Particular = item.Particular,
                                Date = item.Date,
                                Amount = item.Amount,
                                AdjAmt = item.AdjAmt,
                                TBalAmt = item.TBalAmt,
                                IsDeleted = item.IsDeleted ?? false
                            };

                            _context.billAdjustmentDetails.Add(billItem);
                        }

                        _context.SaveChanges(); 
                    }


                    TempData["SuccessMessage"] = "Bill Adjustment added successfully!";
                    return RedirectToAction("BillAdjustmentIndex"); //
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving data: " + ex.Message);
                }
            }

            
            return View("Index", model);
        }




        [HttpPost]
        public IActionResult DeleteBillAdjustment(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Balance Id.");
            }

            var response = _service.DeleteBillAdjustment(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }
        public JsonResult GetVoucherDetailsByParty(string party)
        {
            // Fetch the voucher and bill details based on the selected party
            var vouchers = (from vd in _context.voucherDetails
                            join v in _context.Vouchers on vd.VoucherId equals v.VoucherId
                            where vd.BillToParty == party 
                            select new
                            {
                                VoucherNumber = v.VoucherNumber,
                                VoucherDate = v.VoucherDate
                            }).ToList();

            var bills = _context.voucherDetails
                            .Where(vd => vd.BillToParty == party && vd.BalAmount > 0)
                            .Select(vd => new
                            {
                                BillNumber = vd.BillNumber
                            }).Distinct().ToList();

            var firstVoucher = vouchers.FirstOrDefault();

            return Json(new
            {
                vouchers = vouchers.Select(v => new { voucherNumber = v.VoucherNumber }),
                voucherDate = firstVoucher != null ? firstVoucher.VoucherDate.ToString("yyyy-MM-dd") : null,
                bills = bills.Select(b => new { billNumber = b.BillNumber })
            });
        }

        [HttpPost]
        public JsonResult GetVoucherDetails(string voucherNumber)
        {
            var result = (from v in _context.Vouchers
                          join vd in _context.voucherDetails
                          on v.VoucherId equals vd.VoucherId
                          where v.VoucherNumber == voucherNumber
                          select new VoucherDto
                          {
                              // Fields from Voucher
                              VoucherId = v.VoucherId,
                              VoucherNumber = v.VoucherNumber,
                              VoucherDate = v.VoucherDate,
                              TotalAmount = v.TotalAmount,

                              // Fields from VoucherDetail
                              AccountDescription = vd.AccountDescription,
                              CurrentBalance = vd.CurrentBalance,
                              BillNumber = vd.BillNumber,
                              Amount=vd.Amount,
                              BalAmount = vd.BalAmount
                          }).ToList();

            return Json(result);
        }


    }
}
