using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class AddressBookMasterEntity
    {
        [Key]
        public int ClientId { get; set; }

        public string? ClientName { get; set; }

        public string? BillParty { get; set; }

        public string? ContactPersonName { get; set; }

        public string? Mobile { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string ?Country { get; set; }

        public string? Pincode { get; set; }

        public string? GSTNo { get; set; }

        public bool? IsDeleted { get; set; } = false;
        public bool IsActive { get; set; }

        
        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
