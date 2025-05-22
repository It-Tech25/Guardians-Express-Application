using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    using System.ComponentModel.DataAnnotations;

    public class FinancialLedgerEntity
    {
        [Key]
        public int Id { get; set; }
        public int? LedgerId { get; set; }
        public string? FinancialYear { get; set; }
        public string? AccountHead { get; set; }
        public string? BalanceIn { get; set; }
        public decimal? OpeningBalance { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }


    }
}

