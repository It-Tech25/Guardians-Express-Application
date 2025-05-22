using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GuardiansExpress.Controllers
{
    public class CreditNoteController : Controller
    {
        private readonly ICreditNoteService _creditNoteService;
        private readonly MyDbContext _context;

        public CreditNoteController(ICreditNoteService creditNoteService, MyDbContext context)
        {
            _creditNoteService = creditNoteService;
            _context = context;
        }

        [HttpGet]
        public IActionResult CreditNoteIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;
            ViewBag.s = _context.branch.Where(a => a.IsDeleted == false).ToList();

            var query = _context.creditNotes
                        .Where(a => a.IsDeleted == false &&
                                   (string.IsNullOrEmpty(searchTerm) ||
                                    a.DN_CN_No.Contains(searchTerm) ||
                                    a.BillNo.Contains(searchTerm) ||
                                    a.Branch.Contains(searchTerm) ||
                                    a.AccHead.Contains(searchTerm)));

            int totalRecords = query.Count();

            var creditNotes = query.OrderByDescending(x => x.CreatedAt)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

            var creditNoteModels = creditNotes.Select(MapToModel).ToList();

            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(creditNoteModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreditNoteAdd(CreditNoteModel creditNoteModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage)
                                         .ToList();
                    TempData["ErrorMessage"] = $"Validation errors: {string.Join(", ", errors)}";
                    return RedirectToAction("CreditNoteIndex");
                }

                // Set additional fields
                creditNoteModel.IsDeleted = false;
                creditNoteModel.IsActive = true;
                creditNoteModel.CreatedAt = DateTime.Now;
                creditNoteModel.UpdatedAt = DateTime.Now;
                creditNoteModel.UpdatedBy = HttpContext.Session.GetString("Username") ?? "System";

                var creditNoteEntity = MapToEntity(creditNoteModel);
                var response = _creditNoteService.CreateCreditNote(creditNoteEntity);

                if (response.statuCode == 1)
                {
                    TempData["SuccessMessage"] = "Credit note created successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = response.message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("CreditNoteIndex");
        }

        [HttpGet]
        public IActionResult GetCreditNote(int id)
        {
            try
            {
                var creditNoteEntity = _creditNoteService.GetCreditNoteById(id);
                if (creditNoteEntity == null)
                {
                    return NotFound();
                }

                var creditNoteModel = MapToModel(creditNoteEntity);
                return Json(creditNoteModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreditNoteUpdate(CreditNoteModel creditNoteModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage)
                                         .ToList();
                    TempData["ErrorMessage"] = $"Validation errors: {string.Join(", ", errors)}";
                    return RedirectToAction("CreditNoteIndex");
                }

                var existingEntity = _creditNoteService.GetCreditNoteById(creditNoteModel.Id);
                if (existingEntity == null)
                {
                    TempData["ErrorMessage"] = "Credit note not found.";
                    return RedirectToAction("CreditNoteIndex");
                }

                // Update fields
                existingEntity.Branch = creditNoteModel.Branch;
                existingEntity.NoteDate = creditNoteModel.NoteDate;
                existingEntity.NoteType = creditNoteModel.NoteType;
                existingEntity.DN_CN_No = creditNoteModel.DN_CN_No;
                existingEntity.AccHead = creditNoteModel.AccHead;
                existingEntity.BillNo = creditNoteModel.BillNo;
                existingEntity.BillDate = creditNoteModel.BillDate;
                existingEntity.SalesType = creditNoteModel.SalesType;
                existingEntity.BillAmount = creditNoteModel.BillAmount;
                existingEntity.SelectAddress = creditNoteModel.SelectAddress;
                existingEntity.AccGSTIN = creditNoteModel.AccGSTIN;
                existingEntity.Address = creditNoteModel.Address;
                existingEntity.NoGST = creditNoteModel.NoGST;
                existingEntity.UpdatedAt = DateTime.Now;
                existingEntity.UpdatedBy = HttpContext.Session.GetString("Username") ?? "System";

                var response = _creditNoteService.UpdateCreditNote(existingEntity);
                if (response.statuCode == 1)
                {
                    TempData["SuccessMessage"] = "Credit note updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = response.message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("CreditNoteIndex");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var response = _creditNoteService.DeleteCreditNote(id);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = "Credit note deleted successfully" });
                }
                return Json(new { success = false, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private CreditNoteEntity MapToEntity(CreditNoteModel model)
        {
            return new CreditNoteEntity
            {
                Id = model.Id,
                Branch = model.Branch,
                NoteDate = model.NoteDate,
                NoteType = model.NoteType,
                DN_CN_No = model.DN_CN_No,
                AccHead = model.AccHead,
                BillNo = model.BillNo,
                BillDate = model.BillDate,
                SalesType = model.SalesType,
                BillAmount = model.BillAmount,
                SelectAddress = model.SelectAddress,
                AccGSTIN = model.AccGSTIN,
                Address = model.Address,
                NoGST = model.NoGST,
                IsDeleted = model.IsDeleted,
                IsActive = model.IsActive,
                UpdatedBy = model.UpdatedBy,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }

        private CreditNoteModel MapToModel(CreditNoteEntity entity)
        {
            return new CreditNoteModel
            {
                Id = entity.Id,
                Branch = entity.Branch,
                NoteDate = entity.NoteDate,
                NoteType = entity.NoteType,
                DN_CN_No = entity.DN_CN_No,
                AccHead = entity.AccHead,
                BillNo = entity.BillNo,
                BillDate = entity.BillDate,
                SalesType = entity.SalesType,
                BillAmount = entity.BillAmount,
                SelectAddress = entity.SelectAddress,
                AccGSTIN = entity.AccGSTIN,
                Address = entity.Address,
                NoGST = entity.NoGST,
                IsDeleted = entity.IsDeleted,
                IsActive = entity.IsActive,
                UpdatedBy = entity.UpdatedBy,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}