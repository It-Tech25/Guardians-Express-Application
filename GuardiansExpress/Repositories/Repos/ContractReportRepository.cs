using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories
{
    public class ContractReportRepository : IContractReportRepository
    {
        private readonly MyDbContext _context;

        public ContractReportRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<ContractEntity> GetAll(string? branchName = null, string? referenceName = null, string? invoiceType = null, string? contractType = null, bool? tempClose = null)
        {
            var query = _context.contractEntities
                .Where(c => c.IsDeleted == false)
                .AsQueryable();

            // Apply filters
            //if (!string.IsNullOrWhiteSpace(branchName))
            //{
            //    query = query.Where(c => c.BranchName != null && c.BranchName.Contains(branchName));
            //}

            //if (!string.IsNullOrWhiteSpace(referenceName))
            //{
            //    query = query.Where(c => c.ReferenceName != null && c.ReferenceName.Contains(referenceName));
            //}

            //if (!string.IsNullOrWhiteSpace(invoiceType))
            //{
            //    query = query.Where(c => c.InvoiceType != null && c.InvoiceType.Equals(invoiceType, StringComparison.OrdinalIgnoreCase));
            //}

            //if (!string.IsNullOrWhiteSpace(contractType))
            //{
            //    query = query.Where(c => c.ContractType != null && c.ContractType.Equals(contractType, StringComparison.OrdinalIgnoreCase));
            //}

            //if (tempClose.HasValue)
            //{
            //    query = query.Where(c => c.TempClose == tempClose.Value);
            //}

            return query.OrderBy(c => c.ContractId).ToList();
        }

        public List<string> GetUniqueBranchNames()
        {
            return _context.contractEntities
                .Where(c => c.IsDeleted == false && !string.IsNullOrEmpty(c.BranchName))
                .Select(c => c.BranchName!)
                .Distinct()
                .OrderBy(b => b)
                .ToList();
        }

        public List<string> GetUniqueReferenceNames()
        {
            return _context.contractEntities
                .Where(c => c.IsDeleted == false && !string.IsNullOrEmpty(c.ReferenceName))
                .Select(c => c.ReferenceName!)
                .Distinct()
                .OrderBy(r => r)
                .ToList();
        }

        public List<string> GetUniqueInvoiceTypes()
        {
            return _context.contractEntities
                .Where(c => c.IsDeleted == false && !string.IsNullOrEmpty(c.InvoiceType))
                .Select(c => c.InvoiceType!)
                .Distinct()
                .OrderBy(i => i)
                .ToList();
        }

        public List<string> GetUniqueContractTypes()
        {
            return _context.contractEntities
                .Where(c => c.IsDeleted == false && !string.IsNullOrEmpty(c.ContractType))
                .Select(c => c.ContractType!)
                .Distinct()
                .OrderBy(ct => ct)
                .ToList();
        }
    }
}