using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GuardiansExpress.Repositories.Repos
{
    public class CountryAndStateRepo : ICountryAndStateRepo
    {
        private readonly MyDbContext _context;

        public CountryAndStateRepo(MyDbContext context)
        {
            _context = context;
        }



        public IEnumerable<CountryEntity> Country()
        {
            return _context.countryEntities.ToList();
        }



        public IEnumerable<StateDTO> StatebyContry()
        {
            var sc = from c in _context.countryEntities
                     join s in _context.stateEntities
                     on c.Id equals s.CountryId
                      //where s.IS == false  
                     select new StateDTO
                     {
                         CountryId = c.Id,
                         Id=s.Id,
                         CountryName = c.CountryName,
                         StateName = s.StateName,

                     };

            return sc.ToList();




        }
    }
}
