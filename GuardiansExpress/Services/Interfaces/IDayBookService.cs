using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Services
{
    public interface IDayBookService
    {
        List<DayBookDTO> GetDayBookEntries(DateTime startDate, DateTime endDate, string branch = "All",
            string transactionType = "All", string accHead = "", string balType = "", string bookType = "", string txnType ="");
    }
}