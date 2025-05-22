using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
   
    public class PurchaseDetailsEntity
    {
           [Key]
        public int PurchaseId { get; set; }
        public int? BranchId { get; set; }
        public DateTime VoucherDate { get; set; }
        public string? ClientName { get; set; }
        public bool? NoGST { get; set; }
        public string? PaymentTerms { get; set; }
        public string? DeliveryTerms { get; set; }
        public string ?Packing { get; set; }
        public string ?ShipTo { get; set; }
        public string? Transport { get; set; }
        public decimal? Insurance { get; set; }
        public decimal? Freight { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string ?IndentNo { get; set; }
        public string? CostCenter { get; set; }
        public decimal? DiscountOnMRP { get; set; }
        public string? ItemName { get; set; }
        public int? FreeQuantity { get; set; }
        public int? Stock { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? RoundOff { get; set; }
        public decimal? NetAmount { get; set; }
        public string ?Notes { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
