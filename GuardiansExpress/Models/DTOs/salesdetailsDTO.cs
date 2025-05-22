namespace GuardiansExpress.Models.DTOs
{
    public class salesdetailsDTO
    {
        public int ItemId { get; set; }

        //public int? InvoiceId { get; set; }

        public decimal? Rate { get; set; }

        public decimal? Amount { get; set; }

        public int? TaxId { get; set; }

        public decimal? Tax_Amt { get; set; }

        public decimal? Total { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }
        public decimal FreightAmount { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public string GRNo { get; set; }
        public string HSCSSN { get; set; }

        public DateTime? GRDate { get; set; }

        public decimal? DetentionAmount { get; set; }

        public int? DetentionDays { get; set; }

        public decimal? OtherCharges { get; set; }

        public string Remarks { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
