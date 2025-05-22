namespace GuardiansExpress.Models.DTOs
{
    public class PurchaseOrderItemModel
    {
        
            public int ItemId { get; set; }
            public int? PurchaseId { get; set; }
            public string? Item { get; set; }
            public string? ItemDescription { get; set; }
            public string? HSNSAC { get; set; }
            public decimal? MRP { get; set; }
            public decimal? Rate { get; set; }
            public decimal? DiscountPercentage { get; set; }
            public int? Quantity { get; set; }
            public int? FreeQuantity { get; set; }
            public int? Stock { get; set; }
            public string? Unit { get; set; }
            public decimal? Amount { get; set; }
            public decimal? TaxPercentage { get; set; }
            public decimal? TaxAmount { get; set; }
            public decimal? TotalAmount { get; set; }
        }
    }


