using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class CreditNoteRepo : ICreditNoteRepo
    {
        private readonly MyDbContext _context;

        public CreditNoteRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CreditNoteModel> GetCreditNotes(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from creditNote in _context.creditNotes
                        where creditNote.IsDeleted == false
                        select new CreditNoteModel
                        {
                            Id = creditNote.Id,
                            Branch = creditNote.Branch,
                            NoteDate = creditNote.NoteDate,
                            NoteType = creditNote.NoteType,
                            DN_CN_No = creditNote.DN_CN_No,
                            AccHead = creditNote.AccHead,
                            BillNo = creditNote.BillNo,
                            BillDate = creditNote.BillDate,
                            SalesType = creditNote.SalesType,
                            BillAmount = creditNote.BillAmount,
                            SelectAddress = creditNote.SelectAddress,
                            AccGSTIN = creditNote.AccGSTIN,
                            Address = creditNote.Address,
                            NoGST = creditNote.NoGST,
                            CreatedAt = creditNote.CreatedAt,
                            UpdatedAt = creditNote.UpdatedAt
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(cn => cn.DN_CN_No.Contains(searchTerm) ||
                                         cn.BillNo.Contains(searchTerm) ||
                                         cn.AccHead.Contains(searchTerm) ||
                                         cn.SelectAddress.Contains(searchTerm)
                                   );
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public CreditNoteEntity GetCreditNoteById(int id)
        {
            return _context.creditNotes.Find(id);
        }

        public GenericResponse CreateCreditNote(CreditNoteEntity creditNote)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                creditNote.IsDeleted = false;
                creditNote.IsActive = true;
                creditNote.CreatedAt = DateTime.Now;
                creditNote.UpdatedAt = DateTime.Now;

                _context.creditNotes.Add(creditNote);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Credit Note created successfully";
                response.currentId = creditNote.Id;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Credit Note: " + ex.Message;
                response.currentId = 0;
                response.statuCode = 0;
            }
            return response;
        }

        public GenericResponse UpdateCreditNote(CreditNoteEntity creditNote)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.creditNotes.Where(c => c.Id == creditNote.Id).FirstOrDefault();
                if (existing != null)
                {
                    creditNote.IsActive = true;
                    creditNote.IsDeleted = false;
                    creditNote.UpdatedAt = DateTime.Now;
                    creditNote.CreatedAt = existing.CreatedAt;
                    creditNote.NoteDate = existing.NoteDate;
                    creditNote.SalesType = existing.SalesType;// Preserve the original creation date

                    _context.Entry(existing).CurrentValues.SetValues(creditNote);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Credit Note updated successfully";
                    response.currentId = creditNote.Id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Credit Note not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Credit Note: " + ex.Message;
                response.currentId = 0;
                response.statuCode = 0;
            }
            return response;
        }

        public GenericResponse DeleteCreditNote(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.creditNotes.Where(c => c.Id == id).FirstOrDefault();
                if (existing != null)
                {
                    existing.IsDeleted = true;
                    existing.UpdatedAt = DateTime.Now;

                    _context.Update(existing);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Credit Note deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed: Credit Note not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Credit Note: " + ex.Message;
                response.currentId = 0;
                response.statuCode = 0;
            }
            return response;
        }
    }
}