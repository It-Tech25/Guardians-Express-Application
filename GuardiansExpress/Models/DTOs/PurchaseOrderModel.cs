namespace GuardiansExpress.Models.DTOs
{
    public class PurchaseOrderModel
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
            public string? Insurance { get; set; }
            public string? Freight { get; set; }
            public DateTime? ValidFrom { get; set; }
            public DateTime? ValidTo { get; set; }
            public string? IndentNo { get; set; }
            public string? CostCenter { get; set; }
            public decimal? DiscountOnMRP { get; set; }
            public string? Notes { get; set; }
            public decimal? GrossAmount { get; set; }
            public decimal? Discount { get; set; }
            public decimal? Tax { get; set; }
            public decimal? RoundOff { get; set; }
            public decimal? NetAmount { get; set; }
            public DateTime? CreatedOn { get; set; }
            public DateTime? UpdatedOn { get; set; }
             public bool? IsActive { get; set; }
             public string? BranchName { get; set; }

             public bool? IsDeleted { get; set; }

    }
    }


