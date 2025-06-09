using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using System.Diagnostics.Contracts;

namespace GuardiansExpress.Repositories
{
    public interface IContractReportRepository
    {
        List<ContractEntity> GetAll(string? branchName = null, string? referenceName = null, string? invoiceType = null, string? contractType = null, bool? tempClose = null);
        List<string> GetUniqueBranchNames();
        List<string> GetUniqueReferenceNames();
        List<string> GetUniqueInvoiceTypes();
        List<string> GetUniqueContractTypes();
    }
}