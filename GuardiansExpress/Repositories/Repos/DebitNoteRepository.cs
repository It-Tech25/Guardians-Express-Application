using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuardiansExpress.Repositories
{
    public class DebitNoteRepository : IDebitNoteRepository
    {
        private readonly MyDbContext _context;

        public DebitNoteRepository(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DebitNoteFilterDTO> GetByFilterAsync(DebitNoteFilterDTO filter)
        {
            var debitNoteList = _context.creditNotes
                .Where(s => s.IsDeleted == false && s.IsActive == true)
                .Select(s => new DebitNoteFilterDTO
                {
                    Id = s.Id,
                    Branch = s.Branch,
                    NoteDate = s.NoteDate,
                    NoteType = s.NoteType,
                    DN_CN_No = s.DN_CN_No,
                    AccHead = s.AccHead,
                    BillNo = s.BillNo,
                    BillDate = s.BillDate,
                    SalesType = s.SalesType,
                    BillAmount = s.BillAmount,
                    SelectAddress = s.SelectAddress,
                    AccGSTIN = s.AccGSTIN,
                    Address = s.Address,
                    NoGST = s.NoGST,
                    IsDeleted = s.IsDeleted,
                    IsActive = s.IsActive,
                    UpdatedBy = s.UpdatedBy,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToList();

            return debitNoteList;
        }
    }
}


