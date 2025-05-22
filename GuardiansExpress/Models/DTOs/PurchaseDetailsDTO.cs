namespace GuardiansExpress.Models.DTOs
{
    public class PurchaseDetailsDTO
    {
        public int PurchaseId { get; set; }

        public string? Branch { get; set; }
        public int? BranchId { get; set; }

        public DateTime VoucherDate { get; set; }

        public string? ClientName { get; set; }

        public bool? NoGST { get; set; }

        public string? PaymentTerms { get; set; }

        public string? DeliveryTerms { get; set; }

        public string? Packing { get; set; }

        public string? ShipTo { get; set; }

        public string? Transport { get; set; }

        public decimal? Insurance { get; set; }

        public decimal? Freight { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public string? IndentNo { get; set; }

        public string? CostCenter { get; set; }

        public decimal? DiscountOnMRP { get; set; }

        public string? ItemName { get; set; }

        public string? Description { get; set; }

        public string? HSN_SAC { get; set; }

        public decimal? MRP { get; set; }

        public decimal? Rate { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public int Quantity { get; set; }

        public int? FreeQuantity { get; set; }

        public int? Stock { get; set; }

        public string? Unit { get; set; }

        public decimal? Amount { get; set; }

        public decimal? TaxPercentage { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? GrossAmount { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Tax { get; set; }

        public decimal? RoundOff { get; set; }

        public decimal? NetAmount { get; set; }

        public string? Notes { get; set; }
        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public int? UpdatedBy { get; set; }

        public int? id { get; set; }

        public string? BranchName { get; set; }

    }



    public class PurchaseDetail
    {
        public int PurchaseId { get; set; }
        public int? BranchId { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string? ClientName { get; set; }
        public bool? NoGST { get; set; }
        public string? PaymentTerms { get; set; }
        public string? DeliveryTerms { get; set; }
        public string? Packing { get; set; }
        public string? ShipTo { get; set; }
        public string? Transport { get; set; }
        public decimal? Insurance { get; set; }
        public decimal? Freight { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? IndentNo { get; set; }
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
        public string? Notes { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public int? UpdatedBy { get; set; }
    }

}
