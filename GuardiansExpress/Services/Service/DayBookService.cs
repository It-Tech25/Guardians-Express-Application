using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Repository;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class DayBookService : IDayBookService
    {
        private readonly IDayBookRepository _repository;

        public DayBookService(IDayBookRepository repository)
        {
            _repository = repository;
        }

        public List<DayBookDTO> GetDayBookEntries(DateTime startDate, DateTime endDate,
            string branch, string transactionType, string accHead, string balType, string bookType, string TxnType)
        {
            return _repository.GetDayBookEntries(startDate, endDate, branch, transactionType, accHead, balType, bookType);
        }
    }
}
