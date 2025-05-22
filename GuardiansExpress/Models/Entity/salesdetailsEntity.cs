using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuardiansExpress.Models.Entity
{
    public class salesdetailsEntity
    {
        [Key]
        public int ItemId { get; set; }

        public int? InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual InvoiceEntity Invoice { get; set; }


        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public int? TaxId { get; set; }
        public decimal? Tax_Amt { get; set; }
        public decimal? Total { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public string GRNo { get; set; }
        public string HSCSSN { get; set; }
        public DateTime? GRDate { get; set; }
        public decimal? DetentionAmount { get; set; }
        public int? DetentionDays { get; set; }
        public decimal? OtherCharges { get; set; }
        public string Remarks { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
