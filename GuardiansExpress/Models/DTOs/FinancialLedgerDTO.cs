using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class FinancialLedgerDTO
    {
        [Key]
        public int Id { get; set; }
        public string? AccHead { get; set; }
        public string? FinancialYear { get; set; }
        public string? AccountHead { get; set; }
        public string? BalanceIn { get; set; }
        public decimal? OpeningBalance { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}
