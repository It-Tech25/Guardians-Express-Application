using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class TrialBalanceDTO
    {
        public string Branch { get; set; }
        public string AccHead { get; set; }
        public string Group { get; set; }
        public string SubGroup { get; set; }

        public decimal? OpeningDebit { get; set; }
        public decimal? OpeningCredit { get; set; }

        public decimal? FreightAmount { get; set; }
        public decimal? HireAmount { get; set; }

        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }

        public DateTime? Date { get; set; } // For filtering by DateFrom / DateTo
    }
}

