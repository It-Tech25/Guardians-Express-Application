using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuardiansExpress.Models.DTOs
{
    public class CreditNoteModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [StringLength(100, ErrorMessage = "Branch cannot exceed 100 characters")]
        [Display(Name = "Branch")]
        public string? Branch { get; set; }

        [Required(ErrorMessage = "Note date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Note Date")]
        public DateTime? NoteDate { get; set; }

        [Required(ErrorMessage = "Note type is required")]
        [StringLength(50, ErrorMessage = "Note type cannot exceed 50 characters")]
        [Display(Name = "Note Type")]
        public string? NoteType { get; set; }

        [Required(ErrorMessage = "Document number is required")]
        [StringLength(50, ErrorMessage = "Document number cannot exceed 50 characters")]
        [Display(Name = "DN/CN Number")]
        public string? DN_CN_No { get; set; }

        [Required(ErrorMessage = "Account head is required")]
        [StringLength(100, ErrorMessage = "Account head cannot exceed 100 characters")]
        [Display(Name = "Account Head")]
        public string? AccHead { get; set; }

        [StringLength(50, ErrorMessage = "Bill number cannot exceed 50 characters")]
        [Display(Name = "Bill Number")]
        public string? BillNo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Bill Date")]
        public DateTime? BillDate { get; set; }

        [StringLength(50, ErrorMessage = "Sales type cannot exceed 50 characters")]
        [Display(Name = "Sales Type")]
        public string? SalesType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Bill amount must be a positive number")]
        [DataType(DataType.Currency)]
        [Display(Name = "Bill Amount")]
        public decimal? BillAmount { get; set; }

        [StringLength(50, ErrorMessage = "Address selection cannot exceed 50 characters")]
        [Display(Name = "Select Address")]
        public string? SelectAddress { get; set; }

        [StringLength(15, ErrorMessage = "GSTIN cannot exceed 15 characters")]
        [RegularExpression(@"^[0-9A-Za-z]{15}$", ErrorMessage = "Invalid GSTIN format")]
        [Display(Name = "Account GSTIN")]
        public string? AccGSTIN { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "No GST")]
        public bool? NoGST { get; set; }

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; } = false;

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; } = true;

        [StringLength(100, ErrorMessage = "Updated by cannot exceed 100 characters")]
        [Display(Name = "Updated By")]
        public string? UpdatedBy { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}