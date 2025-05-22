using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuardiansExpress.Models.Entity
{
    public class InvoiceEntity
    {
        [Key]
        public int InvoiceId { get; set; }

        public int? BranchId { get; set; }

        public int? InvTypeId { get; set; }

        public int? SNo { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTime? InvDate { get; set; }

        public bool? GSTType { get; set; }

        public int? ClientId { get; set; }

        public string? SelectAddress { get; set; }

        public string? AccGSTIN { get; set; }

        public string? Address { get; set; }

        public string? SelectContact { get; set; }

        public string? ContactPerson { get; set; }

        public string? ClientEmail { get; set; }

        public string? ClientMobile { get; set; }

        public string? OrderNo { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? PONo { get; set; }

        public DateTime? PODate { get; set; }

        public DateTime? DueDate { get; set; }

        public string? ShipToSelectAddress { get; set; }

        public string? ShipToGSTIN { get; set; }

        public string? ShipToAddress { get; set; }

        public string? Mode { get; set; }

        public string? VehicleNo { get; set; }

        public string? GREwayNo { get; set; }

        public DateTime? GRDate { get; set; }

        public string? EwayBillNo { get; set; }

        public string? Packages { get; set; }

        public string? Transporter { get; set; }

        public string? TransporterId { get; set; }

        public int? DispatchFromState { get; set; }

        public string? DispatchFromCity { get; set; }

        public int? DispatchFromPincode { get; set; }

        public string? DispatchFrom { get; set; }

        public string? CostCenter { get; set; }

        public string? ChallanNo { get; set; }

        public string? PaymentTerm { get; set; }


        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public decimal? GrossAmount { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Tax { get; set; }

        public decimal? RoundOff { get; set; }

        public decimal? NetAmount { get; set; }

        [InverseProperty("Invoice")]
        public virtual List<salesdetailsEntity> BillItems { get; set; } = new List<salesdetailsEntity>();


    }
}
