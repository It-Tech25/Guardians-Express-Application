using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class PlaceRepo : IPlaceRepo
    {
        private readonly MyDbContext _context;

        public PlaceRepo(MyDbContext context)
        {
            _context = context;
        }

        //----------------------------- Place Master -------------------------------------------

        public IEnumerable<Place> GetPlaces(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.placeEntity
                .Where(p => p.IsDeleted == false && (string.IsNullOrEmpty(searchTerm) || p.PlaceName.Contains(searchTerm)))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return query;
        }

        public Place GetPlaceById(int id)
        {
            return _context.placeEntity.Find(id);
        }

        public GenericResponse CreatePlace(Place place)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                place.IsDeleted = false;
                _context.placeEntity.Add(place);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Place created successfully";
                response.currentId = place.Id;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Place: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse UpdatePlace(Place place)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingPlace = _context.placeEntity.FirstOrDefault(p => p.Id == place.Id);
                if (existingPlace != null)
                {
                    place.IsDeleted = false;
                    _context.Entry(existingPlace).CurrentValues.SetValues(place);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Place updated successfully";
                    response.currentId = place.Id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Place not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Place: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeletePlace(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingPlace = _context.placeEntity.FirstOrDefault(p => p.Id == id);
                if (existingPlace != null)
                {
                    existingPlace.IsDeleted = true;
                    _context.Update(existingPlace);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Place deleted successfully";
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
                response.message = "Failed to delete Place: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
