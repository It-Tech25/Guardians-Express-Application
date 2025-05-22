using System;
using System.ComponentModel.DataAnnotations;


namespace GuardiansExpress.Models.Entity
{
    
    public class CreditNoteEntity
    {
        [Key]
        public int Id { get; set; }

        public string? Branch { get; set; }

        public DateTime? NoteDate { get; set; } = DateTime.UtcNow;

      
        public string? NoteType { get; set; }

      
        public string? DN_CN_No { get; set; }

        public string? AccHead { get; set; }

        public string? BillNo { get; set; }

        public DateTime? BillDate { get; set; }

        public string? SalesType { get; set; }

       
        public decimal? BillAmount { get; set; } = 0.00m;

    
        public string? SelectAddress { get; set; }

    
        public string? AccGSTIN { get; set; }

        public string? Address { get; set; }

        public bool? NoGST { get; set; } = false; // false = GST applicable, true = No GST

        public bool? IsDeleted { get; set; } = false;  // False = Not deleted, True = Deleted (Soft Delete)

   
        public bool? IsActive { get; set; } = true;  // True = Active, False = Inactive

  
        public string? UpdatedBy { get; set; } // Stores last updated user ID or username

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
