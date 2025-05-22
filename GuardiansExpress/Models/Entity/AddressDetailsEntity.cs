using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class AddressDetailsEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("LedgerMasterEntity")]  
        public int? LedgerId { get; set; }

        public string? BranchName { get; set; }
        public string? NameAddressMobile { get; set; }
        public string? Address { get; set; }
        public string? CityStatePincode { get; set; }
        public string? GSTIN { get; set; }

        public virtual LedgerMasterEntity LedgerMasterEntity { get; set; }
    }
}
