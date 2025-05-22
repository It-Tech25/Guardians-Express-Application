using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class EXP_CREDITNoteEntity
    {
        [Key]
        public int ExpenceId { get; set; }

        public int? Branch { get; set; }

        public string? BranchCode { get; set; }
        public DateTime? NoteDate { get; set; }

        public string? InvoiceNo { get; set; }

        public int? AccHead { get; set; }

        public string? CostCenter { get; set; }

        public string? Remarks { get; set; }

        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

    }
}

