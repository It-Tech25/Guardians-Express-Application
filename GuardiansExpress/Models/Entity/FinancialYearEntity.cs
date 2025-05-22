using System.ComponentModel.DataAnnotations;

    namespace GuardiansExpress.Models.Entity
    {
        public class FinancialyearEntity
        {
            [Key]
            public int FinancialYearId { get; set; }
            public int? FromYear { get; set; }
            public int? ToYear { get; set; }
            public DateTime? StartDate { get; set; }

            public DateTime? EndDate { get; set; }

            public bool? IsDefault { get; set; }

            public string? Branch { get; set; }

            public bool? IsDeleted { get; set; }

            public bool? IsActive { get; set; }

            public DateTime? CreatedOn { get; set; }

            public int? CreatedBy { get; set; }

            public DateTime? UpdatedOn { get; set; }

            public int? UpdatedBy { get; set; }

            public int? InvoiceTypeId { get; set; }


    }
    }
