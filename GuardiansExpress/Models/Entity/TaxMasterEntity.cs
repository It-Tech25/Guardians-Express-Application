using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class TaxMasterEntity
    {
        [Key]
        public int TaxId { get; set; }

        public string? TaxName { get; set; }

        public string? TaxType { get; set; }

        public string? Status { get; set; }

        public bool? IsCommonTax { get; set; }

        public bool? IsTCSTax { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
