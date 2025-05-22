using GuardiansExpress.Models.DTOs;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IGRReportService
    {
       // IEnumerable<GRDTOs> GetAllGRReports();
        IEnumerable<GRDTOs> Getgrdetails(int? branchId, string fromDate, string toDate, string fromGRNo, string toGRNo, string status);
    }
}