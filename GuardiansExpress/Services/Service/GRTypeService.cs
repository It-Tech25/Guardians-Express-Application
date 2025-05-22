// GRTypeService.cs (Service Implementation)
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using GuardiansExpress.Models.Entity;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class GRTypeService : IGRTypeService
    {
        private readonly IGRTypeRepo _grTypeRepo;
        private readonly MyDbContext _context;

        public GRTypeService(IGRTypeRepo grTypeRepo,MyDbContext context)
        {
            _grTypeRepo = grTypeRepo;
            _context = context;

        }
        public IEnumerable<GRType> GetAllGRTypes()
        {
            var res = _context.gRTypes.Where(a => a.IsDeleted == false).ToList();
            return res;
        }
        public IEnumerable<GRType> GetAllGRTypes(string searchTerm, int pageNumber, int pageSize)
        {
            return _grTypeRepo.GetGRTypes(searchTerm, pageNumber, pageSize);
        }

        public GRType GetGRType(int id)
        {
            return _grTypeRepo.GetGRTypeById(id); 
        }

        public GenericResponse AddGRType(GRType grType)
        {
            return _grTypeRepo.CreateGRType(grType);
        }

        public GenericResponse EditGRType(GRType grType)
        {
            return _grTypeRepo.UpdateGRType(grType);
        }

        public GenericResponse RemoveGRType(int id)
        {
            return _grTypeRepo.DeleteGRType(id);
        }
    }
}