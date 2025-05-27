using GuardiansExpress.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuardiansExpress.Models.DTOs
{
    public class DebitNoteFilterDTO
    {
        public int Id { get; set; }


        public string? Branch { get; set; }
        public string? branchname { get; set; }



        public DateTime? NoteDate { get; set; }

      
        public string? NoteType { get; set; }

        public string? DN_CN_No { get; set; }

        public string? AccHead { get; set; }

      
        public string? BillNo { get; set; }


        public DateTime? BillDate { get; set; }

       
        public string? SalesType { get; set; }

        public decimal? BillAmount { get; set; }

        public string? SelectAddress { get; set; }

        
        public string? AccGSTIN { get; set; }

        public string? Address { get; set; }

        public bool? NoGST { get; set; }

       
        public bool? IsDeleted { get; set; } = false;

        public bool? IsActive { get; set; } = true;

       
        public string? UpdatedBy { get; set; }

       
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

