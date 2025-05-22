using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class FinancialBillEntity
    {
        [Key]
        public int BillId { get; set; }

        public int? BillTypeId { get; set; }

        public string? BillSeries { get; set; }

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
