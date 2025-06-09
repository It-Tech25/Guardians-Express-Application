using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services
{
    public class ContractReportService : IContractReportService
    {
        private readonly IContractReportRepository _contractRepository;

        public ContractReportService(IContractReportRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public List<ContractEntity> GetAll(string? branchName = null, string? referenceName = null, string? invoiceType = null, string? contractType = null, bool? tempClose = null)
        {
            return _contractRepository.GetAll(branchName, referenceName, invoiceType, contractType, tempClose);
        }

        public List<string> GetUniqueBranchNames()
        {
            return _contractRepository.GetUniqueBranchNames();
        }

        public List<string> GetUniqueReferenceNames()
        {
            return _contractRepository.GetUniqueReferenceNames();
        }

        public List<string> GetUniqueInvoiceTypes()
        {
            return _contractRepository.GetUniqueInvoiceTypes();
        }

        public List<string> GetUniqueContractTypes()
        {
            return _contractRepository.GetUniqueContractTypes();
        }
    }
}