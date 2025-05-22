using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class CompanySetupMasterEntity
    {
        [Key]
        public int Id { get; set; }

        public string? CompanyName { get; set; }

        public string? InvoicePrefix { get; set; }

        public string? CNPrefix { get; set; }

        public string? ONPrefix { get; set; }

        public string? AdvanceAgainstGRDispatch { get; set; }

        public string? PaymentReceipt { get; set; }  // Corrected this line

        public bool? IsDeleted { get; set; }
    }
}
