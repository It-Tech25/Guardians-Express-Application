using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
    public class BodyTypeRepo : IBodyTypeRepo
    {
        private readonly MyDbContext _context;

        public BodyTypeRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Body Type-------------------------------------------

        public IEnumerable<BodyTypeEntity> GetBodyTypes()
        {
            // Retrieve all body types that are not marked as deleted
            var bodyTypes = _context.BodyTypes.Where(b => b.IsDeleted == false).ToList();
            return bodyTypes;
        }

        public GenericResponse CreateBodyType(BodyTypeEntity bodyType)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                if (string.IsNullOrEmpty(bodyType.Bodytype))
                {
                    response.statuCode = 0;
                    response.message = "Body Type cannot be null or empty.";
                    response.currentId = 0;
                    return response;
                }
                bodyType.IsDeleted = false; // Set the default value for IsDeleted to false
                _context.BodyTypes.Add(bodyType); // Add the body type entity to the context
                _context.SaveChanges();  // Save changes to the database
                response.statuCode = 1;  // Success status code
                response.message = "Body Type created successfully"; // Success message
                response.currentId = bodyType.Id; // Return the ID of the created body type
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Body Type: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }

        public BodyTypeEntity GetBodyTypeById(int id)
        {
            // Retrieve a body type by its ID
            return _context.BodyTypes.Find(id);
        }

        public GenericResponse UpdateBodyType(BodyTypeEntity bodyType)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing body type by its ID
                var existingBodyType = _context.BodyTypes.Where(b => b.Id == bodyType.Id).FirstOrDefault();
                if (existingBodyType != null)
                {
                    // Ensure IsDeleted is set to false
                    bodyType.IsDeleted = false;

                    // Update the entity
                    _context.Entry(existingBodyType).CurrentValues.SetValues(bodyType);
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1;  // Success status code
                    response.message = "Body Type updated successfully"; // Success message
                    response.currentId = bodyType.Id; // Return the ID of the updated body type
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Body Type not found"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the body type is not found
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Body Type: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
                return response;
            }
        }

        public GenericResponse DeleteBodyType(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing body type by its ID
                var existingBodyType = _context.BodyTypes.Where(b => b.Id == id).FirstOrDefault();
                if (existingBodyType != null)
                {
                    // Mark the body type as deleted
                    existingBodyType.IsDeleted = true;
                    _context.Update(existingBodyType); // Update the body type in the context
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1; // Success status code
                    response.message = "Body Type deleted successfully"; // Success message
                    response.currentId = id; // Return the ID of the deleted body type
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Delete Failed"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the body type is not found
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Body Type: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }
    }
}
