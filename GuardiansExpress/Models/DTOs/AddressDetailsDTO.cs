using GuardiansExpress.Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuardiansExpress.Models.DTOs
{
    public class AddressDetailsDTO
    {
        public int Id { get; set; }

        public int? LedgerId { get; set; }

        public string? BranchName { get; set; }
        public string? NameAddressMobile { get; set; }
        public string? Address { get; set; }
        public string? CityStatePincode { get; set; }
        public string? GSTIN { get; set; }
        public virtual LedgerMasterEntity LedgerMasterEntity { get; set; }
    }
}



   
