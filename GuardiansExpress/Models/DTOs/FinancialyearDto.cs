using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class FinancialyearDto
    {
        [Key]  // Marks the primary key
        public int FinancialYearId { get; set; }

        [Range(1900, 9999, ErrorMessage = "From Year must be a valid year between 1900 and 9999.")]
        public int? FromYear { get; set; }

        [Range(1900, 9999, ErrorMessage = "To Year must be a valid year between 1900 and 9999.")]
        public int? ToYear { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public bool? IsDefault { get; set; }

        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters.")]
        public string? Branch { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public int? InvoiceTypeId { get; set; }

        public List<FinancialInvoiceDto>? fiv { get; set; }

        public List<FinancialBillDto>? fib { get; set; }
    }
}
