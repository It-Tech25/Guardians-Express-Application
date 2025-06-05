using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public interface IContractReportService
    {
        List<ContractReportViewModel> GetContractReports(int? branchId, string accHead, string referenceName, string invoiceType, string contractType);
        List<BranchMasterEntity> GetAllBranches();
        List<string> GetAllAccHeads();
    }
}