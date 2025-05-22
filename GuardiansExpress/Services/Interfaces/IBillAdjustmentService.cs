using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IBillAdjustmentService
    {
        IEnumerable<BillAdjustmentDTO> GetBillAdjustment(string searchTerm, int pageNumber, int pageSize);
        BillAdjustmentEntity GetBillAdjustmentById(int id);
        GenericResponse CreateBillAdjustment(BillAdjustmentEntity Bill);
        GenericResponse UpdateBillAdjustment(BillAdjustmentEntity Bill);
        GenericResponse DeleteBillAdjustment(int id);


    }
}
