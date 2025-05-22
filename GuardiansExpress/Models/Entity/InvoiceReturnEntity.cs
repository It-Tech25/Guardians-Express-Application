using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class InvoiceReturnEntity
    {
        [Key]
        public int InvoiceReturnID { get; set; }


        public int? BranchID { get; set; }

        public int? AddressID { get; set; }

        public DateTime? InvoiceReturnDate { get; set; }
        public string? AccHead { get; set; }
        public string? InvoiceNo { get; set; }
        public string? SalesType { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string? Address { get; set; }
        public string ?AccGSTIN { get; set; }
        public string? CostCenter { get; set; }
        public bool? NoGST { get; set; }
        public bool? DiscountOnMRP { get; set; }
        public string? Notes { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? RoundOff { get; set; }
        public decimal? NetAmount { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }
    }
}