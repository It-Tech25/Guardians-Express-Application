using System.ComponentModel.DataAnnotations;
namespace GuardiansExpress.Models.Entity
{
	public class VehicleTypeEntity
    {
        [Key]
        public int Id { get; set; }
      
        public string? VehicleType { get; set; }
		public string? VehicleImage { get; set; }
		public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool ?IsActive { get; set; }
    }
}
