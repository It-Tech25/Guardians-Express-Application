using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class CountryEntity
    {
        [Key]
        public int Id { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
    }

    
}
