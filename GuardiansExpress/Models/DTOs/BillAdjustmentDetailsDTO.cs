namespace GuardiansExpress.Models.DTOs
{
    public class BillAdjustmentDetailsDTO
    {
        public int BillId { get; set; }

        public int? BalanceFID { get; set; }

        public int? RefNo { get; set; }

        public string? Particular { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AdjAmt { get; set; }

        public decimal? TBalAmt { get; set; }

        public bool? IsDeleted { get; set; }
      
    }
}
