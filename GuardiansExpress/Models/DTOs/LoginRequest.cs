using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email Id required")]
        public string? emailId { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public string? password { get; set; }
    }
}
