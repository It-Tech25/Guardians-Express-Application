using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTO
{
    public class GroupHeadModel
    {
        public int Id { get; set; }

       
        public string? GroupHeadName { get; set; }

     
        public string? Behaviour { get; set; }

       
        public string? OrderOfPLBs { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }
}
