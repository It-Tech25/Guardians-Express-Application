using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class VehicleGroupEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? VehicleGroup { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

      

    }
}
