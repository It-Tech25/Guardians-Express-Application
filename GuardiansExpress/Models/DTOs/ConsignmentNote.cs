namespace GuardiansExpress.Models.DTOs
{
    public class ConsignmentNote
    {
        public string TransporterId { get; set; }
        public string PAN { get; set; }
        public string LrNumber { get; set; }
        public DateOnly? LrDate { get; set; }
        public string TruckNumber { get; set; }
        public string TruckType { get; set; }

        // Consignor details
        public string ConsignorName { get; set; }
        public string ConsignorAddress { get; set; }
        public string ConsignorState { get; set; }
        public string ConsignorGSTIN { get; set; }

        // Consignee details
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeState { get; set; }
        public string ConsigneeGSTIN { get; set; }

        // Shipment details
        public int? NoOfPackages { get; set; }
        public string Description { get; set; }
        public string? GrossWeight { get; set; }
        public string?  LoadWeight { get; set; }
        public Decimal? InvoiceValue { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string EWayBillNumber { get; set; }
        public string EWayBillDate { get; set; }
        public string ExpiryDate { get; set; }
    }
}
