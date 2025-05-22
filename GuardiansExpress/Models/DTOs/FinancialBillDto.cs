namespace GuardiansExpress.Models.DTOs
{
    public class FinancialBillDto
    {
        public int BillId { get; set; }

        public int? BillTypeId { get; set; }

        public string? BillSeries { get; set; }

        public int? FinancialYearId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Branch { get; set; }

    }
}
