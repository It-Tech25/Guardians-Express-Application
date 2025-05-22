using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class FinancialInvoiceEntity
    {
        [Key]
        public int InvoiceId { get; set; }

        public int? InvoiceTypeId { get; set; }

        public string? InvoiceSeries { get; set; }

        public int? FinancialYearId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Branch { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
