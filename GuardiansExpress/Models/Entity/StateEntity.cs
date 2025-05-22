using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class StateEntity
    {
        [Key]
        public int Id { get; set; }
        public string? StateName { get; set; }

        public int? CountryId { get; set; }
    }  
    
    public class StateDTO
    {
        
        public int Id { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }

        public int? CountryId { get; set; }
    }
}
