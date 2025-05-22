using GuardiansExpress.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

public class VehicleReportViewModel
{
    public VehicleReportDTO Filter { get; set; } = new VehicleReportDTO();

    public List<SelectListItem> Branches { get; set; }
    public List<SelectListItem> VehicleTypes { get; set; }
    public List<SelectListItem> VehicleGroups { get; set; }
    public List<SelectListItem> Owners { get; set; }
    public List<SelectListItem> DocumentTypes { get; set; }
    public List<SelectListItem> StatusList { get; set; }
    public List<SelectListItem> EMIStatusList { get; set; }
    public List<SelectListItem> Financiers { get; set; }

    public List<string> ReportTypes { get; set; } = new List<string>
    {
        "Vehicle Report",
        "Document Report",
        "Stock Report",
        "Document Report Exp.Dt. Wise",
        "Group Wise Vehicle",
        "Waiting Report",
        "Vehicle EMI Summary"
    };
}
