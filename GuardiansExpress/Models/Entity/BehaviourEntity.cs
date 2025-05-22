using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class BehaviourEntity
    {
        [Key]
        public int BehaviourID { get; set; }
        public string? BehaviourName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}

