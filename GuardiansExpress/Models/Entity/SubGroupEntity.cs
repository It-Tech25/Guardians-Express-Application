using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuardiansExpress.Models.Entity
{
    public class SubGroupEntity
    {

        [Key]
        public int subgroupId { get; set; }
        

        public string? SubGroupName { get; set; }

        public int? GroupId { get; set; }

        public bool? Detailed { get; set; }

        public bool? AcceptAddress { get; set; }

        public bool? Employee { get; set; }

        public bool? BalanceDashboard { get; set; }

        public string? orderin { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}

