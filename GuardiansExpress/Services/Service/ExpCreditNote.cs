using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Repositories.Repos;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
   
        public class ExpCreditNote : IExpcredit
    {
            private readonly IExpCreditNoteRepo _expCreditNoteRepo;

            public ExpCreditNote(IExpCreditNoteRepo expCreditNoteRepo)
            {
                _expCreditNoteRepo = expCreditNoteRepo;
            }

            public IEnumerable<Exp_credit> GetCreditNotes()
        {
                return _expCreditNoteRepo.GetCreditNotes();
            }

            public EXP_CREDITNoteEntity GetCreditNoteById(int id)
            {
                return _expCreditNoteRepo.GetCreditNoteById(id);
            }

            public GenericResponse AddCreditNote(EXP_CREDITNoteEntity entity)
            {
                if (entity.ExpenceId == 0)
                {
                    return _expCreditNoteRepo.AddCreditNote(entity);
                }
                else
                {
                    return _expCreditNoteRepo.UpdateCreditNote(entity);
                }
            }

            public GenericResponse DeleteCreditNoteById(int id)
            {
                return _expCreditNoteRepo.DeleteCreditNoteById(id);
            }
        }
    

}

