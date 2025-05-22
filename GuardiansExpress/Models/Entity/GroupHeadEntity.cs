using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class GroupHeadEntity
    {

        [Key]
        public int Id { get; set; }


        public string? GroupHeadName { get; set; }

        public string? Behaviour { get; set; }

        public string? OrderOfPLBs { get; set; }

        public bool? IsDeleted { get; set; } = false;  
    }   
}
