using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class InvoiceReturnDetailsEntity
    {
        [Key]
        public int InvoiceReturnDetailID { get; set; }
        public int InvoiceReturnID { get; set; }

        public string? ItemDescription { get; set; }
        public string? HSN_SAC { get; set; }
        public string? InvReturnAcc { get; set; }
        public decimal? MRP { get; set; }
        public decimal? Rate { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public int? Qty { get; set; }
        public int? FreeQty { get; set; }
        public string? Unit { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? Total { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }


    }
}