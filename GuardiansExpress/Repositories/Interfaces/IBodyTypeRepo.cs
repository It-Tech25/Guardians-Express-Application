using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IBodyTypeRepo
    {
        // Method to get all body types that are not marked as deleted
        IEnumerable<BodyTypeEntity> GetBodyTypes();

        // Method to create a new body type
        GenericResponse CreateBodyType(BodyTypeEntity bodyType);

        // Method to get a specific body type by its ID
        BodyTypeEntity GetBodyTypeById(int id);

        // Method to update an existing body type
        GenericResponse UpdateBodyType(BodyTypeEntity bodyType);

        // Method to delete a body type by marking it as deleted
        GenericResponse DeleteBodyType(int id);
    }
}
