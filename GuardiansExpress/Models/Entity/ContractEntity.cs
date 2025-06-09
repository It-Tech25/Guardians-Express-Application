using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class ContractEntity
    {
        [Key]
        public int ContractId { get; set; }
        public int? BranchMasterId { get; set; } 
        public int? InvoiceId { get; set; }
         
        public bool? DisableContract { get; set; } = false;
        public bool? AutoInvoice { get; set; } = false;
        public bool? TempClose { get; set; } = false;

        public string? ClientName { get; set; }
        public string? BranchName { get; set; }

        public string? ReferenceName { get; set; }
        public string? InvoiceType { get; set; }

        public string? ContractType { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }

        public bool? EndRental { get; set; } = false;

        public bool? EmailReminder { get; set; } = false;
        public bool? SMSReminder { get; set; } = false;
        public bool? WhatsAppReminder { get; set; } = false;

  

        public bool? IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;

    }
}
