namespace GuardiansExpress.Models.Entity
{
    public class Place
    {
        public int Id { get; set; }                // Primary Key
        public string? PlaceName { get; set; }     // Place name
        public bool? IsDeleted { get; set; }       // Soft delete flag
    }
}
