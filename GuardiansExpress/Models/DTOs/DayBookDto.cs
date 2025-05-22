using System;

namespace GuardiansExpress.Models.DTOs
{
    public class DayBookDTO
    {
        public int SrNo { get; set; }
        public DateTime Date { get; set; }
        public string? ReferenceNo { get; set; }
        public string? AccountHead { get; set; }
        public string? Particulars { get; set; }
        public string? VoucherNo { get; set; }
        public string? ChequeNo { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string? Branch { get; set; }
        public string? TransactionType { get; set; }
        public string? BalanceAmount { get; set; }
        public string? BookType { get; set; }
        public string? TxnType { get; set; }
    }
}