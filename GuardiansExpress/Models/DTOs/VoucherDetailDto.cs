using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class VoucherDetailDTO
    {
        public int DetailId { get; set; }

        [Required]
        public int VoucherId { get; set; }

        [StringLength(255)]
        public string? AccountDescription { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? CurrentBalance { get; set; }

        [StringLength(500)]
        public string? Particular { get; set; }

        [StringLength(100)]
        public string? BillToParty { get; set; }

        [StringLength(50)]
        public string? BillNumber { get; set; }

        [StringLength(20)]
        public string? VehicleNumber { get; set; }

        // Transaction Types
        [StringLength(20)]
        public string? FromTransactionType { get; set; } // "Dr (Withdrawal)"

        [StringLength(20)]
        public string? ToTransactionType { get; set; } // "Cr (Deposit)"

        // Amount Fields
        [Range(0, double.MaxValue)]
        public decimal? Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? BalAmount { get; set; }

        // Bank Transfer Fields (for Contra vouchers)
        [StringLength(100)]
        public string? FromBank { get; set; }

        [StringLength(100)]
        public string? ToBank { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TransferAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? BalanceAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ToBalanceAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ReceiveAmount { get; set; }

        // Debit/Credit Indicator
        [StringLength(10)]
        public string? DrCrType { get; set; } // "Dr" or "Cr"
    }
}