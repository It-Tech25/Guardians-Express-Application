namespace GuardiansExpress.Models.DTOs
{
    public class UserTypeMasterDto
    {
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}