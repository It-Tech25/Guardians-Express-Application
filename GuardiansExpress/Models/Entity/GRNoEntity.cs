using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class GRNoEntity
    {
        [Key]
    public int Grnoid { get; set; }

        public string GR { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? createdOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

    
    }
}
