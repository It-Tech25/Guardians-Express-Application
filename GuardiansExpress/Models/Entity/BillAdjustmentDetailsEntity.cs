using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuardiansExpress.Models.Entity
{
    public class BillAdjustmentDetailsEntity
    {
        [Key]
        public int BillId { get; set; }

        public int? BalanceFID { get; set; }
        [ForeignKey("BalanceFID")]
        public virtual BillAdjustmentEntity BillAdjustment { get; set; }

        public int? RefNo { get; set; }

        public string? Particular { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AdjAmt { get; set; }

        public decimal? TBalAmt { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
