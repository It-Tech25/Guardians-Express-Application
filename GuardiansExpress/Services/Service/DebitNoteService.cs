using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repositories;
using GuardiansExpress.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardiansExpress.Services
{
    public class DebitNoteService : IDebitNoteService
    {
        private readonly IDebitNoteRepository _debitNoteRepository;

        public DebitNoteService(IDebitNoteRepository debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
        }

        public IEnumerable<DebitNoteFilterDTO> GetByFilterAsync(DebitNoteFilterDTO filter)
        {
            // Fetch data from repository
            var debitNotes = _debitNoteRepository.GetByFilterAsync(filter);


            // Map entities to DTOs (using AutoMapper or manual mapping)
            var debitNoteDtos = new List<DebitNoteFilterDTO>();
            //foreach (var note in debitNotes)
            //{
            //    debitNoteDtos.Add(new DebitNoteFilterDTO
            //    {

            //        Branch = note.Branch,
            //        CreatedOn = note.CreatedOn,
            //        CreatedBy = note.CreatedByUser?.Username  // Example of mapping related data
            //    });
            //}

            return debitNoteDtos;
        }
    }
}

