using GuardiansExpress.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.DTOs
{
    public class LedgerMasterDTO
    {
        [Key]
        public int LedgerId { get; set; }
        public string? subgroupName { get; set; }
        public string? AccHead { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public decimal? BalanceOpening { get; set; }
        public string? AccGroup { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? BankAccount { get; set; }
        public bool? TaxLedger { get; set; }
        public bool? Taxable { get; set; }
        public bool? VehicleExpense { get; set; }
        public decimal? TDSPercent { get; set; }
        public string? BalanceIn { get; set; }
        public decimal? OpeningBalance { get; set; }
        public string? UserName { get; set; }
        public string? ContactPerson { get; set; }
        public string? AltMobile { get; set; }
        public string? TelNo { get; set; }
        public string? RefID { get; set; }
        public string? CCEmailId { get; set; }
        public string? OtherEmailId { get; set; }
        public string? VendorCode { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public List<AddressDetailsDTO> AddressDetails { get; set; } = new List<AddressDetailsDTO> { };
        public List<FinancialLedgerEntity> FinancialLedgers { get; set; } = new List<FinancialLedgerEntity> { };
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? RegistrationType { get; set; }
        public string? PartyType { get; set; }
        public string? CINNo { get; set; }
        public string? GSTIN { get; set; }
        public string? PANNo { get; set; }
        public string? AAdharNumber { get; set; }
        public int? Pincode { get; set; }
        public string? AccHolderName { get; set; }
        public string? BankName { get; set; }
        public string? BankAccNo { get; set; }
        public string? BankBranch { get; set; }
        public string? IFSCCode { get; set; }
        public string? PaymentTerm { get; set; }
        public int? DueDays { get; set; }
        public string? Agent { get; set; }

        public string? Password { get; set; }
        public string? BranchName { get; set; }
        public string? NameAddressMobile { get; set; }
        public string? Address { get; set; }
        public string? CityStatePincode { get; set; }
        public bool? WalkinLedger { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
