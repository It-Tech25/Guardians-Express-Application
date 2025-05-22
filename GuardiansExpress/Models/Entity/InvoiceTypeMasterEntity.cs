    using System.ComponentModel.DataAnnotations;

    namespace GuardiansExpress.Models.Entity
    {
        public class InvoiceTypeMasterEntity
        {
            [Key]
            public int Id { get; set; }
            [Required(ErrorMessage = "Invoice Type Required")]
            public string? InvoiceType { get; set; }
            [Required(ErrorMessage = "Status Required")]
            public string? Status { get; set; }
            public bool? IsDeleted { get; set; } 
            public bool? IsActive { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? LastUpdatedBy { get; set; }
            public DateTime? LastUpdatedOn { get; set; }
        }
    }
