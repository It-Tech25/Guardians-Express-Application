

namespace GuardiansExpress.Models.DTOs
{
    public class VehicleGroupModel
    {
        public int Id { get; set; }
        public string? VehicleGroup { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
