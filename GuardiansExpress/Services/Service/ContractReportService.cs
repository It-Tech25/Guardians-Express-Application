using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Services
{
    public class ContractReportService : IContractReportService
    {
        private readonly IContractReportRepository _repository;

        public ContractReportService(IContractReportRepository repository)
        {
            _repository = repository;
        }

        public List<ContractReportViewModel> GetContractReports(int? branchId, string accHead, string referenceName, string invoiceType, string contractType)
        {
            return _repository.GetContractReports(branchId, accHead, referenceName, invoiceType, contractType);
        }

        public List<BranchMasterEntity> GetAllBranches()
        {
            return _repository.GetAllBranches();
        }

        public List<string> GetAllAccHeads()
        {
            return _repository.GetAllAccHeads();
        }
    }
}