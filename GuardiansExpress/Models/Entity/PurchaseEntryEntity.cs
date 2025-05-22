using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class PurchaseDetails
    {
        [Key]
        public int Id { get; set; }
        public string? Branch { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? ClientName { get; set; }
       
        public string? Address { get; set; }
        public string? GSTIN { get; set; }
        public bool? NoGST { get; set; }
        public bool? DiscountOnMRP { get; set; }
        public string? PONo { get; set; }
        public DateTime? PODate { get; set; }
        public string? ChInNo { get; set; }
        public DateTime? ChInDate { get; set; }
        public string? CostCenter { get; set; }
        public string? EwaybillNo { get; set; }
        //public string Items { get; set; }
        public string? ItemName { get; set; }
        //public string ItemDetails { get; set; }
        public string? State { get; set; }   // Capitalized
        public int? Qty { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxAmountTotal { get; set; }
        public decimal? RoundOff { get; set; }
        public decimal? NetAmount { get; set; }

        public string? Notes { get; set; }
        public bool IsDeleted { get; set; }
    }
}
