using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class BodyTypeEntity
    {
        [Key]
        public int Id { get; set; }


        public string? Bodytype { get; set; }

        public bool ?IsDeleted { get; set; }
    }
}
