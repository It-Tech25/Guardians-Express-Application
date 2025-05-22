namespace GuardiansExpress.Models.DTOs
{
    public class BillSubmissionReportDTO
    {
        public int BillSubmissionId { get; set; }
        public string? ClientName { get; set; }
        public string? BillNo { get; set; }
        public DateTime? BillSubDate { get; set; }
        public string? BillSubmissionBy { get; set; }
        public string? ReceivedBy { get; set; }
        public string? HandedOverBy { get; set; }
        public string? DocketNo { get; set; }
        public string? CourierName { get; set; }
        public bool IsActive { get; set; }

    }
}
