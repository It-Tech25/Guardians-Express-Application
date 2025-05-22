using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class InvoiceGrEntity
    {
        [Key]
        public int Invoiceid { get; set; }

        public int? GRId { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public decimal? InvoiceValue { get; set; }

        public string? EwayBillNo { get; set; }
        public string? InvoiceNo { get; set; }

        public DateTime? EwayBillExpiredate { get; set; }

        public bool? Isdeleted { get; set; }

        public bool? IsActive { get; set; }

    }
}
