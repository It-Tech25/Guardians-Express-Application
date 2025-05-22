namespace GuardiansExpress.Models.DTOs
{
    public class VehicleMasterDTO
    {
        public int VehicleId { get; set; }

        public string? VehicleNo { get; set; }

        public int? BranchId { get; set; }

        public int? VehicleTypeId { get; set; }
        public int? BodyTypeId { get; set; }
        public string? Bodytype { get; set; }
        public string? DisplayVehicleNo { get; set; }
        public DateOnly? LRDate { get; set; }
        public decimal? Weight { get; set; }

        public string? OwnedBy { get; set; }

        public string? Transporter { get; set; }

        public string? MaxWeightAllowed { get; set; }

        public bool? Status { get; set; }

        public string? DocumentType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal? Amount { get; set; }

        public string? Remarks { get; set; }

        public string? Uploads { get; set; }

       
        public string? Docs { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        public string? BranchName { get; set; }
        public string? VehicleType { get; set; }
        public List<IFormFile>? Upload { get; set; }



       

    }
}
