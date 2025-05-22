using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class GREntity
    {
        [Key]
        public int GRId { get; set; }

        public string? Branch { get; set; }

        public string? VehicleNo { get; set; }

        public string? OwnedBy { get; set; }
        public string? LoadType { get; set; }
        public string? Grtype { get; set; }

        public string? GRNo { get; set; }

        public DateTime? GRDate { get; set; }

        public string? ClientName { get; set; }

        public int? BillingAddress { get; set; }

        public decimal? FreightAmount { get; set; }
        public string? GrossWeight { get; set; }
        public string? LoadWeight { get; set; }

        public string? Consigner { get; set; }

        public int? FromPlace { get; set; }

        public string? Consignee { get; set; }

        public int? ToPlace { get; set; }

        public string? Transporter { get; set; }

        public decimal? HireRate { get; set; }

        public string? ItemDescription { get; set; }

        public int? Quantity { get; set; }

        public int? IncurencedBy { get; set; }

        public string? InsurenceNo { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
