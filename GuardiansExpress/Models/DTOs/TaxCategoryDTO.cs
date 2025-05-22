using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Models.DTOs
{
    public class TaxCategoryDTO
    {
        public int ID { get; set; }

        public string? CategoryName { get; set; }

      
        
        public string? Status { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
        public List<TaxDetailsTableDTO> TaxDetails { get; set; }
        


    }
}
