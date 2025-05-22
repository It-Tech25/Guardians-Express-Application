namespace GuardiansExpress.Models.DTOs
{
    public class VehicleDocumentsDTO
    {
        public int VehicleDocumentsId { get; set; }

        public string? DocumentType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal? Amount { get; set; }

        public string? Remarks { get; set; }

        public string? Uploads { get; set; }

        public string? Docs { get; set; }

        public int VehicleId { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
