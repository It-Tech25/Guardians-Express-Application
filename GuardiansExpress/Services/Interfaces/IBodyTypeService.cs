using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IBodyTypeService
    {
        IEnumerable<BodyTypeEntity> GetBodyTypes();
        GenericResponse CreateBodyType(BodyTypeEntity bodyType);
        BodyTypeEntity GetBodyTypeById(int id);
        BodyTypeEntity GetBodyTypeByName(string name);
        bool BodyTypeExists(string name);
        GenericResponse UpdateBodyType(BodyTypeEntity bodyType);
        GenericResponse DeleteBodyType(int id);
    }
}