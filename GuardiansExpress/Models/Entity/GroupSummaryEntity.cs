using System;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class GroupSummary
    {
        [Key]
        public int GroupSummaryId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Branch { get; set; }

        public string AccGroup { get; set; }

        public string ReportType { get; set; }

        public string Ledger { get; set; }

        public string BalType { get; set; }

        public decimal Balance { get; set; }

        public bool IsImportant { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
