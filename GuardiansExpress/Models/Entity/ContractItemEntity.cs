using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class ContractItemEntity
    {
        [Key]
        public int ItemId { get; set; }

        public int? ContractId { get; set; }

        public string? MaterialDescription { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string? FromPlace { get; set; }

        public string? ToPlace { get; set; }

        public string? VehicleType { get; set; }

        public decimal? FreightRate { get; set; }

        public string? VehicleSize { get; set; }
    }
}
