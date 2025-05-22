namespace GuardiansExpress.Models.DTOs
{
    public class FinancialInvoiceDto
    {
        public int InvoiceId { get; set; }

        public int? InvoiceTypeId { get; set; }

        public string? InvoiceSeries { get; set; }

        public int? FinancialYearId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Branch { get; set; }

    }
}
