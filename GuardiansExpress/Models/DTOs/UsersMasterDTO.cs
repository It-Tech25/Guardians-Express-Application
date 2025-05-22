using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class UsersMasterDTO
    {
       
        public int UserId { get; set; }
        public string? UserName { get; set; }    
        public string? UserTypeName { get; set; }

        public long? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? Password { get; set; }
        public string? Role { get; set; }

        public string? Country { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public int? Pincode { get; set; }
        public string Address { get; set; }
        public string? AadharCardFrontUrl { get; set; }
        public string? AadharCardBackUrl { get; set; }
        public string? CurrentStatus { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public int? UpdatedBy { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }



    }
}
