using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class AddressBookMasterDTO
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Client name cannot exceed 100 characters.")]
        public string? ClientName { get; set; }

        [StringLength(100, ErrorMessage = "Billing party name cannot exceed 100 characters.")]
        public string? BillParty { get; set; }

        [StringLength(100, ErrorMessage = "Contact person name cannot exceed 100 characters.")]
        public string? ContactPersonName { get; set; }

        [Range(1000000000, 9999999999, ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string? Mobile { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "State name cannot exceed 100 characters.")]
        public string? State { get; set; }

        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
        public string? Country { get; set; }

        [Range(100000, 999999, ErrorMessage = "Please enter a valid pincode.")]
        public string? Pincode { get; set; }

        [StringLength(15, ErrorMessage = "GST number cannot exceed 15 characters.")]
        public string? GSTNo { get; set; }
        [Required]
        public bool? IsDeleted { get; set; } = false;

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [MaxLength(50)]
        public string? UpdatedBy { get; set; }

    }
}
