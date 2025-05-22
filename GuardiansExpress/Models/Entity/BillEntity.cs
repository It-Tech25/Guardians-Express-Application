using System.ComponentModel.DataAnnotations;
namespace GuardiansExpress.Models.Entity
{
    public class BillEntity
    {
        [Key]
        public int Id { get; set; }

        public string? BillType { get; set; }

        public string? Status { get; set; }

        public bool? IsDeleted {  get; set; }
    }
}
