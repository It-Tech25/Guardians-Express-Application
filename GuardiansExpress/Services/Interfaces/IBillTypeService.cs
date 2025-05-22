using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IBillTypeService
    {
        IEnumerable<BillEntity> GetBillMaster();
        GenericResponse CreateBillMaster(BillEntity billType);
        BillEntity GetBillById(int id);
        GenericResponse UpdateBillMaster(BillEntity bill);
        GenericResponse DeleteBillMaster(int id);

    }
}
