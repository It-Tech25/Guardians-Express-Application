// PlaceService.cs (Service Implementation)
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using GuardiansExpress.Models.Entity;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepo _placeRepo;

        public PlaceService(IPlaceRepo placeRepo)
        {
            _placeRepo = placeRepo;
        }

        public IEnumerable<Place> GetAllPlaces(string searchTerm, int pageNumber, int pageSize)
        {
            return _placeRepo.GetPlaces(searchTerm, pageNumber, pageSize);
        }

        public Place GetPlace(int id)
        {
            return _placeRepo.GetPlaceById(id);
        }

        public GenericResponse AddPlace(Place place)
        {
            return _placeRepo.CreatePlace(place);
        }

        public GenericResponse EditPlace(Place place)
        {
            return _placeRepo.UpdatePlace(place);
        }

        public GenericResponse RemovePlace(int id)
        {
            return _placeRepo.DeletePlace(id);
        }
    }
}
