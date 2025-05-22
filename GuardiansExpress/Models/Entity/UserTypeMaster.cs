using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class UserTypeMaster
    {
        [Key]
        public int UserTypeId { get; set; }
        public string? UserTypeName { get; set; }
        public string? Discription { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsShow { get; set; }

       // public int? ParentUserId { get; set; }

        public bool IsActive { get; set; }


    }
}
