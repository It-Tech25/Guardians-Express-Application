namespace GuardiansExpress.Models.DTOs
{
    public class GroupSummaryReportDTO
    {
        public int GroupSummaryId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Branch { get; set; }
        public string AccGroup { get; set; }
        public string ReportType { get; set; }
        public string Ledger { get; set; }
        public string Agent { get; set; }
        public string BalType { get; set; }
        public decimal Balance { get; set; }
        public bool IsImportant { get; set; }
        public bool IsActive { get; set; }
    }
}