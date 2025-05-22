using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class TaxMasterDto
    {
        public int TaxId { get; set; }
        [Required(ErrorMessage = "Ledger field is required.")]
        public string? Ledger { get; set; }
        [Required(ErrorMessage = "TaxName field is required.")]
        public string? TaxName { get; set; }
        [Required(ErrorMessage = "TaxType field is required.")]
        public string? TaxType { get; set; }
        [Required(ErrorMessage = "Status field is required.")]
        public string? Status { get; set; }

        public bool? IsCommonTax { get; set; }

        public bool? IsTCSTax { get; set; }
        public int? Ledgerid { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
