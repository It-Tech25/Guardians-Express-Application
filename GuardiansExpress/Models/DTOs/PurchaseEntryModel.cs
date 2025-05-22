public class PurchaseDto
{
    public int Id { get; set; }
    public string ?Branch { get; set; }
    public string? InvoiceNo { get; set; }
    public string? State { get; set; }   // Capitalized
    public int? Qty { get; set; }
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

    public List<ItemDetail> Items { get; set; } = new List<ItemDetail>();

    public decimal? GrossAmount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmountTotal { get; set; }
    public decimal? RoundOff { get; set; }
    public decimal? NetAmount { get; set; }

    public string? Notes { get; set; }
}

public class ItemDetail
{
    public string? ItemName { get; set; }
    public string? PurchaseAcc { get; set; }
    public string? HSNSAC { get; set; }
    public decimal? MRP { get; set; }
    public decimal? Rate { get; set; }
    public decimal? Discount { get; set; }
    public int? Qty { get; set; }
    public int? FreeQty { get; set; }
    public string? Unit { get; set; }
    public int? PurchaseId { get; set; }
}
