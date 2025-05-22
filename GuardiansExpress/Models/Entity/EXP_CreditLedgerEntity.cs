using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class EXP_CreditLedgerEntity
    {
        [Key]
        public int EXP_CreditLedgerid { get; set; }

        public int? ExpenceId { get; set; }

        public string? ACCDEC { get; set; }

        public string? Particular { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Total { get; set; }

        public bool? IsDeleted { get; set; }

    }
}

