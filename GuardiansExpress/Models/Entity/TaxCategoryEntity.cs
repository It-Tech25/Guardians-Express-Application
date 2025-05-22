using System.ComponentModel.DataAnnotations;
using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Models.Entity
{
    public class TaxCategoryEntity
    {
        [Key]
        public int ID { get; set; }

        public string? CategoryName { get; set; }

        public string? Status { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
       
    }


}
