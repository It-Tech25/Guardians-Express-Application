using System;

namespace GuardiansExpress.Models.DTOs
{
    public class ContractReportViewModel
    {
        // Contract Model Properties
        public int ContractId { get; set; }
        public int? BranchMasterId { get; set; }
        public int? InvoiceId { get; set; }
        public bool? DisableContract { get; set; }
        public bool? AutoInvoice { get; set; }
        public bool? TempClose { get; set; }
        public string ClientName { get; set; }
        public string ReferenceName { get; set; }
        public string InvoiceType { get; set; }
        public string BranchName { get; set; }
        public string ContractType { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string LastInvNo { get; set; }
        public bool? EndRental { get; set; }
        public bool? EmailReminder { get; set; }
        public bool? SMSReminder { get; set; }
        public bool? WhatsAppReminder { get; set; }

        // Contract Item Model Properties
        public int ItemId { get; set; }
        public string MaterialDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string FromPlace { get; set; }
        public string ToPlace { get; set; }
        public string VehicleType { get; set; }
        public decimal? FreightRate { get; set; }
        public string VehicleSize { get; set; }
    }
}