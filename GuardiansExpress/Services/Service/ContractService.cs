using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepo _contractRepo;

        public ContractService(IContractRepo contractRepo)
        {
            _contractRepo = contractRepo;
        }

        public IEnumerable<ContractModel> GetContracts()
        {
            try
            {
                return _contractRepo.GetContracts();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving contracts: " + ex.Message);
            }
        }

        public ContractModel GetContractById(int id)
        {
            try
            {
                return _contractRepo.GetContractById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving contract by ID: " + ex.Message);
            }
        }

        public GenericResponse CreateContract(ContractEntity contract)
        {
            try
            {
                return _contractRepo.CreateContract(contract);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating contract: " + ex.Message);
            }
        }

        public GenericResponse UpdateContract(ContractEntity contract)
        {
            try
            {
                return _contractRepo.UpdateContract(contract);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating contract: " + ex.Message);
            }
        }

        public GenericResponse DeleteContract(int id)
        {
            try
            {
                return _contractRepo.DeleteContract(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting contract: " + ex.Message);
            }
        }
    }
}
