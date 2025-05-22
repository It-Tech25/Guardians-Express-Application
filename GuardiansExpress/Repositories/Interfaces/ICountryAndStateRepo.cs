using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface ICountryAndStateRepo
    {

        IEnumerable<StateDTO> StatebyContry();
        IEnumerable<CountryEntity> Country();
    }
}
