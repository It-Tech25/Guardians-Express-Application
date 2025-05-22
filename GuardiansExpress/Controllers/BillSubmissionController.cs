using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services.Service;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GuardiansExpress.Controllers
{
    public class BillSubmissionController : Controller
    {
        private readonly IBillSubmissionService _billSubmissionService;

        public BillSubmissionController(IBillSubmissionService billSubmissionService)
        {
            _billSubmissionService = billSubmissionService;
        }

        // GET: BillSubmission
        public async Task<IActionResult> BillSubmissionIndex()
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

            var billSubmissions = await _billSubmissionService.GetAllAsync();
            return View(billSubmissions);
        }

        // GET: BillSubmission/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var billSubmission = await _billSubmissionService.GetByIdAsync(id);
                if (billSubmission == null)
                {
                    return NotFound(new { success = false, message = "Bill submission not found" });
                }

                return Json(new
                {
                    success = true,
                    billSubmissionId = billSubmission.BillSubmissionId,
                    billNo = billSubmission.BillNo,
                    billSubDate = billSubmission.BillSubDate,
                    clientName = billSubmission.ClientName,
                    billSubmissionBy = billSubmission.BillSubmissionBy,
                    receivedBy = billSubmission.ReceivedBy,
                    handedOverBy = billSubmission.HandedOverBy,
                    courierName = billSubmission.CourierName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }

        // GET: BillSubmission/Create
        public IActionResult Create()
        {
            var model = new CreateBillSubmissionDTO
            {
                BillSubDate = DateTime.Now
            };
            return View(model);
        }

        // POST: BillSubmission/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBillSubmissionDTO createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = GetCurrentUserId();
                    await _billSubmissionService.CreateAsync(createDto, userId);
                    return Json(new { success = true, message = "Bill submitted successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // If we got this far, something failed
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Validation failed", errors = errors });
        }

        // GET: BillSubmission/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var billSubmission = await _billSubmissionService.GetByIdAsync(id);
            if (billSubmission == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateBillSubmissionDTO
            {
                BillSubmissionId = billSubmission.BillSubmissionId,
                ClientName = billSubmission.ClientName,
                BillNo = billSubmission.BillNo,
                BillSubDate = billSubmission.BillSubDate,
                BillSubmissionBy = billSubmission.BillSubmissionBy,
                ReceivedBy = billSubmission.ReceivedBy,
                HandedOverBy = billSubmission.HandedOverBy,
                DocketNo = billSubmission.DocketNo,
                CourierName = billSubmission.CourierName,
            };

            return View(updateDto);
        }

        // POST: BillSubmission/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateBillSubmissionDTO updateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = GetCurrentUserId();
                    var result = await _billSubmissionService.UpdateAsync(updateDto);
                    if (result == null)
                    {
                        return Json(new { success = false, message = "Bill submission not found" });
                    }
                    return Json(new { success = true, message = "Bill updated successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Validation failed", errors = errors });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return Json(new { success = false, message = "Invalid Bill Submission ID." });
            }

            try
            {
                await _billSubmissionService.DeleteAsync(id);
                return Json(new { success = true, message = "Bill submission deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0; // Default value if user ID not found or not valid
        }
    }
}