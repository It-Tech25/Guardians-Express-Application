using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class BillSubmissionEntity
    {
        [Key]
        public int BillSubmissionId { get; set; } // ✅ Corrected property name

        public string? ClientName { get; set; }
        public string? BillNo { get; set; }
        public DateTime? BillSubDate { get; set; }
        public string? BillSubmissionBy { get; set; }
        public string? ReceivedBy { get; set; }
        public string? HandedOverBy { get; set; }
        public string? DocketNo { get; set; }
        public string? CourierName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }

    }
}
