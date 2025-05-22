using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IBillTypeRepo
    {
        // Get all bills that are not marked as deleted
        IEnumerable<BillEntity> GetBillMaster();

        // Create a new bill
        GenericResponse CreateBillMaster(BillEntity bill);

        // Get a bill by its ID
        BillEntity GetBillById(int id);

        // Update an existing bill
        GenericResponse UpdateBillMaster(BillEntity bill);

        // Delete a bill (soft delete by setting IsDeleted to true)
        GenericResponse DeleteBillMaster(int id);
    }
}
