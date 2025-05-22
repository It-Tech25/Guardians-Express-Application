using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class InvoiceTypeModel
    {
        public int Id { get; set; }
       
        public string? InvoiceType { get; set; }
     
        public string? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
