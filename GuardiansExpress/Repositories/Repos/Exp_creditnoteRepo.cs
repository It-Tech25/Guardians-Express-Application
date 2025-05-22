using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories.Repos
{
    public class ExpCreditNoteRepo: IExpCreditNoteRepo
    {
        private readonly MyDbContext _context;
        public ExpCreditNoteRepo(MyDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Exp_credit> GetCreditNotes()
        {
            var branches = (from b in _context.creditnote
                            join c in _context.branch on b.Branch equals c.id
                            

                            where b.IsDeleted == false && b.IsActive==true
                            select new Exp_credit
                            {
                                ExpenceId = b.ExpenceId,
                                Branch = c.id,
                                BranchName = c.BranchName,
                                NoteDate = b.NoteDate,
                                BranchCode = b.BranchCode,
                                InvoiceNo = b.InvoiceNo,
                                AccHead = b.AccHead,
                                CostCenter = b.CostCenter,
                                Remarks = b.Remarks,
                                IsDeleted = b.IsDeleted,
                                IsActive = b.IsActive,
                            }).ToList();

            return branches;
        }


 
        public EXP_CREDITNoteEntity GetCreditNoteById(int id)
        {
            return _context.creditnote.Where(x=>x.IsDeleted==false).FirstOrDefault();
        }

        public GenericResponse AddCreditNote(EXP_CREDITNoteEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                req.IsDeleted = false;
                req.IsActive = true;
                req.CreatedBy = 1;
                req.CreatedOn = DateTime.Now;
                _context.creditnote.Add(req);
                _context.SaveChanges(); // Save changes first

                response.statuCode = 1;
                response.message = "Add Successful.";
                response.currentId = req.ExpenceId; // Get the inserted ID

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Add failed.";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateCreditNote(EXP_CREDITNoteEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var entity = _context.creditnote.FirstOrDefault(a => a.ExpenceId == req.ExpenceId);
                if (entity == null)
                {
                    response.message = "Record not found";
                    response.currentId = 0;
                    return response;
                }

                _context.Entry(entity).State = EntityState.Detached;
                req.IsDeleted = false;
                req.IsActive = true;
                req.NoteDate = DateTime.Now;
                _context.creditnote.Update(req);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Update Successful.";
                response.currentId = req.ExpenceId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Update failed.";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public GenericResponse DeleteCreditNoteById(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var entity = _context.creditnote.FirstOrDefault(x => x.ExpenceId == id);
                if (entity == null)
                {
                    response.message = "Record not found.";
                    response.currentId = 0;
                    return response;
                }
                entity.IsDeleted = true;
                entity.IsActive = false;
                _context.creditnote.Update(entity);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Delete Successful.";
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Delete failed.";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}