using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class VoucherDetail
    {
        [Key]
        public int DetailId { get; set; }
        public int VoucherId { get; set; }
        public string? AccountDescription { get; set; }
        public decimal? CurrentBalance { get; set; }
        public string? Particular { get; set; }
        public string? BillToParty { get; set; }
        public string? BillNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public decimal? ReceiveAmount { get; set; }
        public string? TransactionType { get; set; }
        public decimal? Amount { get; set; }       
        public decimal? BalAmount { get; set; }    
          
        public Voucher? Voucher { get; set; }
    }
}