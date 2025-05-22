using GuardiansExpress.Models.Entity;

namespace GuardiansExpress.Services.Interfaces
{
    public interface ICountryAndStateService
    {

        IEnumerable<StateDTO> StatebyContry();
        IEnumerable<CountryEntity> Country();
    }
}
