using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;

namespace GuardiansExpress.Services.Service
{
    public class CountryAndStateService : ICountryAndStateService
    {
        private readonly ICountryAndStateRepo _countryAndStateRepo;

        public CountryAndStateService(ICountryAndStateRepo countryAndStateRepo)
        {
            _countryAndStateRepo = countryAndStateRepo;
        }



       public IEnumerable<StateDTO> StatebyContry()
        {
            return _countryAndStateRepo.StatebyContry();
        } 
        
        public IEnumerable<CountryEntity> Country()
        {
            return _countryAndStateRepo.Country();
        }
        

    }
}
