using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class purchaseitemdetailsEntity
    {
        [Key]
        public int itemid { get; set; }

        public string? ItemDescription { get; set; }

        public string? PurchaseAcc { get; set; }

        public string? HSNSAC { get; set; }

        public decimal? MRP { get; set; }

        public decimal? rate { get; set; }

        public decimal? discount { get; set; }

        public int? QTY { get; set; }

        public int? Freeqty { get; set; }

        public string? unit { get; set; }

        public decimal? Amount { get; set; }

        public decimal? tax { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? total { get; set; }

        public int? PurchaseId { get; set; }
    }
}
