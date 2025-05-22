using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class TaxDetailsEntity
    {
        [Key]
        public int TaxID { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
