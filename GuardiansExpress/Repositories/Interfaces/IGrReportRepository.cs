// File: Repositories/Interfaces/IGRReportRepository.cs
using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IGRReportRepository
    {
       // IEnumerable<GRDTOs> GetAllGRReports();
        IEnumerable<GRDTOs> Getgrdetails();
            }
}