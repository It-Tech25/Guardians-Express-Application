using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class GRType
    {
        [Key]
        public int GRTypeId { get; set; }  // ID for GR Type
        public string? TypeName { get; set; }
        public bool? IsDefault { get; set; }  
        public bool? IsDeleted { get; set; }  
    }
}