using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Models.DTOs
{
    public class BillAdjustmentDTO
    {
        public int BalanceId { get; set; }

        public bool? BalanceBills { get; set; }

        
        public string? UnderGroup { get; set; }

        public string? Party { get; set; }

        public string? VoucherNumber { get; set; }

        public DateTime? VoucherDate { get; set; }

        public decimal? BillAmt { get; set; }

        public decimal? BalAmt { get; set; }

        public string? RefDescription { get; set; }

        public string? Particular { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        public int? BillNumber { get; set; }
        public DateTime? Bill_Date { get; set; }
        public List<BillAdjustmentDetailsDTO> BillItems { get; set; } = new List<BillAdjustmentDetailsDTO>();

    }
}
