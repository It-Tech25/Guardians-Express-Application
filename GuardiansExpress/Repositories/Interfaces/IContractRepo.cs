using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;


namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IContractRepo
    {
        IEnumerable<ContractModel> GetContracts();
        ContractModel GetContractById(int id);
        GenericResponse CreateContract(ContractEntity contract);
        GenericResponse UpdateContract(ContractEntity contract);
        GenericResponse DeleteContract(int id);
    }
}
