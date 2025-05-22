using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Repos;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Controllers
{
    public class ExpCreditController : Controller
    {
        private readonly IExpcredit _expCreditNoteRepo;
        private readonly IBranchMasterService _branch;
        private readonly MyDbContext _context;

        public ExpCreditController(IExpcredit expCreditNoteRepo, IBranchMasterService branch, MyDbContext context)
        {
            _expCreditNoteRepo = expCreditNoteRepo;
            _branch = branch;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreditNoteList()
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
            var creditNotes = _expCreditNoteRepo.GetCreditNotes();
            var s = _branch.BranchMasterss().ToList();

            ViewBag.s = s;

            return View(creditNotes);
        }

        [HttpGet]
        public IActionResult GetCreditNoteById(int id)
        {
            var creditNote = _expCreditNoteRepo.GetCreditNoteById(id);
            if (creditNote == null)
            {
                return NotFound();
            }
            return View(creditNote);
        }
        [HttpPost]
        public IActionResult AddOrUpdateCreditNote(Exp_credit model, List<string> AccDesc, List<string> Particular, List<decimal> Amount)
        {
            if (model == null || AccDesc == null || Particular == null || Amount == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            try
            {
                // Save main Exp_credit record
                var newExpCredit = new EXP_CREDITNoteEntity
                {
                    ExpenceId=model.ExpenceId,
                    Branch = model.Branch,
                    BranchCode=model.BranchCode,
                    NoteDate = model.NoteDate,
                    InvoiceNo = model.InvoiceNo,
                    AccHead = model.AccHead,
                    CostCenter = model.CostCenter,
                    Remarks = model.Remarks
                };

                // Call the service layer to add the credit note
                var response = _expCreditNoteRepo.AddCreditNote(newExpCredit);

                if (response.statuCode == 1)
                {
                    int expenceId = response.currentId; // Get the ID of the newly inserted record

                    // Save Ledger Details
                    List<EXP_CreditLedgerEntity> ledgerEntries = new List<EXP_CreditLedgerEntity>();

                    for (int i = 0; i < AccDesc.Count; i++)
                    {
                        var ledgerEntry = new EXP_CreditLedgerEntity
                        {
                            ExpenceId = expenceId, // Link to the parent record
                            ACCDEC = AccDesc[i],
                            Particular = Particular[i],
                            Amount = Amount[i]
                        };

                        ledgerEntries.Add(ledgerEntry);
                    }

                    // Add ledger entries to the database
                    _context.creditledger.AddRange(ledgerEntries);
                    _context.SaveChanges();

                    return RedirectToAction("CreditNoteList");
                }
                else
                {
                    return RedirectToAction("CreditNoteList");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("CreditNoteList");
            }
        }

        //[HttpPost]
        //    public IActionResult AddOrUpdateCreditNote(EXP_CREDITNoteEntity entity)
        //    {

        //            try
        //            {
        //                GenericResponse response;

        //                    response = _expCreditNoteRepo.AddCreditNote(entity);


        //                if (response.statuCode == 1)
        //                {
        //                    return RedirectToAction("CreditNoteList");
        //                }
        //                ModelState.AddModelError("", response.message);
        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError("", "An error occurred while processing your request.");
        //            }

        //        return View("CreditNoteList");
        //    }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Credit Note ID.");
            }

            var res = _expCreditNoteRepo.DeleteCreditNoteById(id);

            if (res.statuCode == 1)
            {
                return Json(new { success = true});
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public JsonResult GetBranchDetails(int branchId)
        {
            var branch = _context.branch
                                 .Where(b => b.id == branchId)
                                 .Select(b => new
                                 {
                                     branchCode = b.BranchCode,
                                     costCenters = _context.branch
                                                           .Where(c => c.id == branchId)
                                                           .Select(c => c.BranchCode)
                                                           .ToList()
                                 })
                                 .FirstOrDefault();

            return Json(branch);
        }

    }
}



