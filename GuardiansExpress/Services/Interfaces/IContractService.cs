using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IContractService
    {
        IEnumerable<ContractModel> GetContracts();
        ContractModel GetContractById(int id);
        GenericResponse CreateContract(ContractEntity contract);
        GenericResponse UpdateContract(ContractEntity contract);
        GenericResponse DeleteContract(int id);
    }
}
