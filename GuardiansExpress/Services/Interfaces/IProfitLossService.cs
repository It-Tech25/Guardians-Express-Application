using GuardiansExpress.Models.DTOs;
using System;

namespace GuardiansExpress.Service
{
    public interface IProfitLossService
    {
        ProfitLossDTO GenerateProfitLossReport(DateTime startDate, DateTime endDate, string branch);
    }
}