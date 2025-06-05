using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories
{
    public class ContractReportRepository : IContractReportRepository
    {
        private readonly MyDbContext _context;

        public ContractReportRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<ContractReportViewModel> GetContractReports(int? branchId, string accHead, string referenceName, string invoiceType, string contractType)
        {
            var query = from contract in _context.Set<ContractModel>()
                        join branch in _context.branch on contract.BranchMasterId equals branch.id into branchGroup
                        from branch in branchGroup.DefaultIfEmpty()
                        join contractItem in _context.Set<ContractItemModel>() on contract.ContractId equals contractItem.ContractId into itemGroup
                        from contractItem in itemGroup.DefaultIfEmpty()
                        where contract.IsDeleted == false && contract.IsActive == true
                        select new ContractReportViewModel
                        {
                            ContractId = contract.ContractId,
                            BranchMasterId = contract.BranchMasterId,
                            BranchName = branch != null ? branch.BranchName : "Head Office",
                            ClientName = contract.ClientName,
                            ReferenceName = contract.ReferenceName,
                            InvoiceType = contract.InvoiceType,
                            ContractType = contract.ContractType,
                            InvoiceNo = contract.InvoiceNo,
                            ContractEndDate = contract.ContractEndDate,
                            LastInvNo = contract.LastInvNo,
                            DisableContract = contract.DisableContract,
                            AutoInvoice = contract.AutoInvoice,
                            TempClose = contract.TempClose,
                            EndRental = contract.EndRental,
                            EmailReminder = contract.EmailReminder,
                            SMSReminder = contract.SMSReminder,
                            WhatsAppReminder = contract.WhatsAppReminder,
                            ItemId = contractItem != null ? contractItem.ItemId : 0,
                            MaterialDescription = contractItem != null ? contractItem.MaterialDescription : "",
                            StartDate = contractItem != null ? contractItem.StartDate : null,
                            EndDate = contractItem != null ? contractItem.EndDate : null,
                            FromPlace = contractItem != null ? contractItem.FromPlace : "",
                            ToPlace = contractItem != null ? contractItem.ToPlace : "",
                            VehicleType = contractItem != null ? contractItem.VehicleType : "",
                            VehicleSize = contractItem != null ? contractItem.VehicleSize : "",
                            FreightRate = contractItem != null ? contractItem.FreightRate : null
                        };

            // Apply filters
            if (branchId.HasValue)
            {
                query = query.Where(x => x.BranchMasterId == branchId.Value);
            }

            if (!string.IsNullOrEmpty(referenceName))
            {
                query = query.Where(x => x.ReferenceName != null && x.ReferenceName.Contains(referenceName));
            }

            if (!string.IsNullOrEmpty(invoiceType))
            {
                query = query.Where(x => x.InvoiceType == invoiceType);
            }

            if (!string.IsNullOrEmpty(contractType))
            {
                query = query.Where(x => x.ContractType == contractType);
            }

            return query.OrderBy(x => x.BranchName).ThenBy(x => x.ClientName).ToList();
        }

        public List<BranchMasterEntity> GetAllBranches()
        {
            return _context.branch
                .Where(b => b.IsDeleted == false && b.IsActive == true)
                .OrderBy(b => b.BranchName)
                .ToList();
        }

        public List<string> GetAllAccHeads()
        {
            return _context.ledgerEntity
                .Where(a => a.IsDeleted == false)
                .Select(a => a.AccHead)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
        }
    }
}