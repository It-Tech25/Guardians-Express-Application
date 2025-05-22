using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class BranchMasterEntity
    {
        [Key]
        public int id { get; set; }

        public bool? status { get; set; }

        public int? Company { get; set; }

        public string? BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? Pincode { get; set; }

        public string? StampImage { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public bool? IsActive { get; set; }

        public int? StateId { get; set; }

        public string? POC { get; set; }
       

    }



    public class BranchMasterDTO
    {

        public int id { get; set; }

        public bool? status { get; set; }

        public int? Company { get; set; }
        public string? CompanyName { get; set; }
        public string? StateName { get; set; }

        public string? BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? Pincode { get; set; }

        public string? StampImage { get; set; }
        public IFormFile? StampImg { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public bool? IsActive { get; set; }

        public int? StateId { get; set; }

        public string? POC { get; set; }

      

    }

}

