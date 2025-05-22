using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class BankReconciliationEntity
    {
        [Key]
        public int BankId { get; set; }

        public string ?BankName { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public string? DocNo { get; set; }

        public DateTime? BankDate { get; set; }

        public string? ChequeNo { get; set; }

        public string? AcDescription { get; set; }

        public string ?RefDescription { get; set; }

        public decimal? Amount { get; set; }

        public string ?Type { get; set; }

        public string ?ReconcileData { get; set; }

        public bool? IsDelete { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }
}
