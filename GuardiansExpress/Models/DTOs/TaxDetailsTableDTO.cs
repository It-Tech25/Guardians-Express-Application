namespace GuardiansExpress.Models.DTOs
{
    public class TaxDetailsTableDTO
    {
        public int TaxID { get; set; }

        public int? TaxCategoryId { get; set; }

        public int? TaxMasterID { get; set; }

        public decimal? Value { get; set; }

        public string? TaxFor { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
