namespace GuardiansExpress.Models.DTO
{
    public class GRTypeDTO
    {
        public int GRTypeId { get; set; }  // ID for GR Type
        public string? TypeName { get; set; }
        public bool? IsDeleted { get; set; }  // Soft delete flag
    }
}