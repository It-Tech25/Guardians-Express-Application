namespace GuardiansExpress.Models.DTOs
{
    public class VoucherDto
    {
        public int VoucherId { get; set; }
        public string? VoucherType { get; set; }
        public string? Branch { get; set; }
        public string? SerialNumber { get; set; }
        public string? VoucherNumber { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string? AccountHead { get; set; }
        public string? ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }


        public int DetailId { get; set; }
        public string? AccountDescription { get; set; }
        public decimal? CurrentBalance { get; set; }
        public string? Particular { get; set; }
        public string? BillToParty { get; set; }
        public string? BillNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public string? TransactionType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BalAmount { get; set; }
        public string? FromBank { get; set; }
        public string? ToBank { get; set; }
        public decimal? TransferAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? ToBalanceAmount { get; set; }
        public decimal? ReceiveAmount { get; set; }
       // public string? DrCrType { get; set; }
        public string? FromTransactionType { get; set; }
        public string? ToTransactionType { get; set; }
        public List<VoucherDetailDTO>? Details { get; set; }
    }
}
