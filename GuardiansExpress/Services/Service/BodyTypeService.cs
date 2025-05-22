using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Services
{
    public class BodyTypeService : IBodyTypeService
    {
        private readonly IBodyTypeRepo _bodyTypeRepo;

        public BodyTypeService(IBodyTypeRepo bodyTypeRepo)
        {
            _bodyTypeRepo = bodyTypeRepo;
        }

        public IEnumerable<BodyTypeEntity> GetBodyTypes()
        {
            return _bodyTypeRepo.GetBodyTypes();
        }

        public BodyTypeEntity GetBodyTypeById(int id)
        {
            return _bodyTypeRepo.GetBodyTypeById(id);
        }

        public BodyTypeEntity GetBodyTypeByName(string name)
        {
            return _bodyTypeRepo.GetBodyTypes()
                .FirstOrDefault(bt => bt.Bodytype.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool BodyTypeExists(string name)
        {
            return _bodyTypeRepo.GetBodyTypes()
                .Any(bt => bt.Bodytype.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public GenericResponse CreateBodyType(BodyTypeEntity bodyType)
        {
            // Additional validation in service layer
            if (BodyTypeExists(bodyType.Bodytype))
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "Body type already exists."
                };
            }

            return _bodyTypeRepo.CreateBodyType(bodyType);
        }

        public GenericResponse UpdateBodyType(BodyTypeEntity bodyType)
        {
            // Check for duplicates excluding current record
            var existing = GetBodyTypeByName(bodyType.Bodytype);
            if (existing != null && existing.Id != bodyType.Id)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "Another body type with this name already exists."
                };
            }

            return _bodyTypeRepo.UpdateBodyType(bodyType);
        }

        public GenericResponse DeleteBodyType(int id)
        {
            return _bodyTypeRepo.DeleteBodyType(id);
        }
    }
}