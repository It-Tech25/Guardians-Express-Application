namespace GuardiansExpress.Models.DTOs
{
    public class CompanyConfigurationViewModel
    {
        public int CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? InfoEmails { get; set; }
        public string? Phone { get; set; }
        public string? TelNo { get; set; }
        public string? Fax { get; set; }
        public string? Website { get; set; }
        public string? PANNo { get; set; }
        public string? GSTIN { get; set; }
        public string? CINNumber { get; set; }
        public string? HSCSSN { get; set; }

        public string? CompanyLogoPath { get; set; }
        public string? LetterHeadImagePath { get; set; }

        public bool? EnableSMS { get; set; }
        public bool? EnableEmail { get; set; }
        public bool? IsDefault { get; set; }
        public bool? ChangeTaxOnInvoice { get; set; }
        public bool? ShowItemImageInLead { get; set; }
        public bool? ShowItemImageInPO { get; set; }
        public bool? EnableKYCVerification { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; } // Nullable DateTime
        public int? CreatedBy { get; set; } // Nullable Int
        public int? UpdatedBy { get; set; } // Nullable Int
        public bool ?IsDeleted { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Pincode { get; set; }

        
        public string? RoundOffInvoice { get; set; }
        public string? RequiredTransportDetails { get; set; }
        public string? ServiceIndustry { get; set; }
        public bool? DuplicateItemInBill { get; set; }
        public bool? ImportGoods { get; set; }
        public bool? BarcodeBilling { get; set; }
        public bool? BarcodeOnSrno { get; set; }
        public bool? ExportTCSApplicable { get; set; }
        public bool? ProvisionForBilling { get; set; }
        public bool? ManualWtInBilling { get; set; }
        public bool? ApprovedVoucherInLedger { get; set; }
        public bool? ManualRate { get; set; }
        public bool? DispatchWiseGR { get; set; }
        public bool? TransShipment { get; set; }
        public bool? ClubVoucherEntryTotal { get; set; }
        public bool? ShowGroupInVoucherEntry { get; set; }
        public bool? DirectDebitDriverTransporter { get; set; }
        public bool? ResetGRNoFinancialYearWise { get; set; }
        public bool? UpcomingEMI { get; set; }
        public bool? LedgerWiseDetail { get; set; }
        public DateTime? LockEntryTill { get; set; } // Nullable DateTime
        public decimal? TCSAmount { get; set; }
        public int ?InvoiceNoMinLength { get; set; }
        public int? ContractDueDays { get; set; }
        public decimal? EwayBillAmount { get; set; }
        public decimal? ORCPercentage { get; set; }
        public TimeSpan? AutoInvoiceTime { get; set; } // Nullable TimeSpan
        public int? PasswordResetDays { get; set; }
        public int? URLExpireTime { get; set; }
        public string? Status { get; set; }
        public string? WhiteListIP { get; set; }

        public string? InvoiceTemplate { get; set; }
        public string? DebitNoteTemplate { get; set; }
        public string? LeadTemplate { get; set; }
        public string? GRTemplate { get; set; }
        public string? GRReceiveTemplate { get; set; }
        public string? BillTemplate { get; set; }
        public string? BillDetails1Template { get; set; }
        public string? BillDetails2Template { get; set; }
        public string? HireTemplate { get; set; }
        public string? HireTransporterTemplate { get; set; }
        public string? SaleOrderTemplate { get; set; }
        public bool? DefaultTemplate { get; set; }
        public string? LeadPDFTitle { get; set; }
        public int? SOTopHeight { get; set; }
        public int? SOHeaderHeight { get; set; }
        public int? SOFooterHeight { get; set; }
        public int? LeadTopHeight { get; set; }
        public int? LeadHeaderHeight { get; set; }
        public int? LeadFooterHeight { get; set; }
        public int? InvTopHeight { get; set; }
        public int? InvHeaderHeight { get; set; }
        public int? InvFooterHeight { get; set; }
        public int? GRTopHeight { get; set; }
        public int? GRHeaderHeight { get; set; }
        public int? GRFooterHeight { get; set; }
        public int? BillTopHeight { get; set; }
        public int? BillHeaderHeight { get; set; }
        public int? BillFooterHeight { get; set; }
        public int? Bill1TopHeight { get; set; }
        public int? Bill1HeaderHeight { get; set; }
        public int? Bill1FooterHeight { get; set; }
        public int? Bill2TopHeight { get; set; }
        public int? Bill2HeaderHeight { get; set; }
        public int? Bill2FooterHeight { get; set; }
        public int? HireTopHeight { get; set; }
        public int? HireHeaderHeight { get; set; }
        public int? HireFooterHeight { get; set; }
        public int? HireTransporterTopHeight { get; set; }
        public int? HireTransporterHeaderHeight { get; set; }
        public int? HireTransporterFooterHeight { get; set; }
        public int? CalDashEventCount { get; set; }

        public string? EInvoiceUsername { get; set; }
        public string? EInvoicePassword { get; set; }
    }
}
