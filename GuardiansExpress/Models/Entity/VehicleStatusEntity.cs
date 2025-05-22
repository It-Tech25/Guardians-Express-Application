using System.ComponentModel.DataAnnotations;
namespace GuardiansExpress.Models.Entity
{
    public class VehicleStatusEntity
    {
        [Key]
        public int VehicleStatusID { get; set; }
        public string? VehicleStatusName { get; set; }
        public string? StatusBehaviour { get; set; }
        public string? Color { get; set; }

    }
}
