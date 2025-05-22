using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class Voucher
    {
        [Key]
        public int VoucherId { get; set; }
        public string? VoucherType { get; set; }
        public string? Branch { get; set; }
        public string? SerialNumber { get; set; }
        public string? VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public string? AccountHead { get; set; }
        public string? ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public DateTime? ReconcileDate { get; set; }
        public string? FromBank { get; set; }
        public string? ToBank { get; set; }
        public decimal? TransferAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? ToBalanceAmount { get; set; }
        public decimal? ReceiveAmount { get; set; }
        //public string? DrCrType { get; set; }
        public string? FromTransactionType { get; set; }
        public string? ToTransactionType { get; set; }
        public ICollection<VoucherDetail>? VoucherDetail { get; set; }
    }
}
