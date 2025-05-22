using System.ComponentModel.DataAnnotations;

public class BillSubmissionDTO
{
    public int BillSubmissionId { get; set; }
    public string? ClientName { get; set; }
    public string? BillNo { get; set; }
    public DateTime? BillSubDate { get; set; }
    public string? BillSubmissionBy { get; set; }
    public string? ReceivedBy { get; set; }
    public string? HandedOverBy { get; set; }
    public string? DocketNo { get; set; }
    public string? CourierName { get; set; }
    public bool IsActive { get; set; }
}

// Create DTO for submission form
public class CreateBillSubmissionDTO
{
    [Required(ErrorMessage = "Client name is required")]
    public string? ClientName { get; set; }

    [Required(ErrorMessage = "Bill number is required")]
    public string? BillNo { get; set; }

    [Display(Name = "Bill Submission Date")]
    [DataType(DataType.Date)]
    public DateTime? BillSubDate { get; set; }

    [Display(Name = "Submitted By")]
    public string? BillSubmissionBy { get; set; }

    [Display(Name = "Received By")]
    public string? ReceivedBy { get; set; }

    [Display(Name = "Handed Over By")]
    public string? HandedOverBy { get; set; }

    [Display(Name = "Docket Number")]
    public string? DocketNo { get; set; }

    [Display(Name = "Courier Name")]
    public string? CourierName { get; set; }

}

// Update DTO
public class UpdateBillSubmissionDTO : CreateBillSubmissionDTO
{
    public int BillSubmissionId { get; set; }
    public bool IsActive { get; set; }
}