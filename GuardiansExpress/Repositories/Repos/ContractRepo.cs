using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;


namespace GuardiansExpress.Repositories.Repos
{
    public class ContractRepo : IContractRepo
    {
        private readonly MyDbContext _context;

        public ContractRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ContractModel> GetContracts()
        {
            var query = from contract in _context.contractEntities
                        where contract.IsDeleted==false
                        select new ContractModel
                        {
                            ContractId = contract.ContractId,
                            ClientName = contract.ClientName,
                            ContractType = contract.ContractType,
                            ContractEndDate = contract.ContractEndDate,
                            EndRental = contract.EndRental,
                            EmailReminder = contract.EmailReminder,
                            SMSReminder = contract.SMSReminder,
                            WhatsAppReminder = contract.WhatsAppReminder,
                            IsActive = contract.IsActive,
                            IsDeleted = contract.IsDeleted,
                            BranchMasterId = contract.BranchMasterId
                        };

            return query.ToList();
        }

        public ContractModel GetContractById(int id)
        {
            var contract = _context.contractEntities
                             .Where(c => c.ContractId == id && c.IsDeleted==false)
                             .FirstOrDefault();

            if (contract == null) return null;

            var branch = _context.branch
                                 .Where(b => b.id == contract.BranchMasterId)
                                 .FirstOrDefault();

            var invoiceType = _context.invoice
                                     .Where(i => i.Id == contract.InvoiceId)
                                     .FirstOrDefault();

            return new ContractModel
            {
                ContractId = contract.ContractId,
                ClientName = contract.ClientName,
                InvoiceType = invoiceType?.InvoiceType, // Use the value from the invoice table
                InvoiceId = contract.InvoiceId, // Make sure ContractModel has this property
                ContractType = contract.ContractType,
                ContractEndDate = contract.ContractEndDate,
                EndRental = contract.EndRental,
                EmailReminder = contract.EmailReminder,
                SMSReminder = contract.SMSReminder,
                WhatsAppReminder = contract.WhatsAppReminder,
                IsActive = contract.IsActive,
                IsDeleted = contract.IsDeleted,
                BranchMasterId = contract.BranchMasterId,
                BranchName = branch?.BranchName
            };
        }

        public GenericResponse CreateContract(ContractEntity contract)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                contract.IsDeleted = false;
                _context.contractEntities.Add(contract);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Contract created successfully";
                response.currentId = contract.ContractId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Contract: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse UpdateContract(ContractEntity contract)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.contractEntities.FirstOrDefault(c => c.ContractId == contract.ContractId);
                if (existing != null)
                {
                   
                    _context.Entry(existing).CurrentValues.SetValues(contract);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Contract updated successfully";
                    response.currentId = contract.ContractId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Contract not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Contract: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeleteContract(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.contractEntities.FirstOrDefault(c => c.ContractId == id);
                if (existing != null)
                {
                    existing.IsDeleted = true;
                    _context.Update(existing);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Contract deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Contract: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
