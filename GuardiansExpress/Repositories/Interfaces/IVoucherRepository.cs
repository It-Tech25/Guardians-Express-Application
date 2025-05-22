using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IVoucherRepository
    {
        IEnumerable<Voucher> GetAllVouchers();
        Task<IEnumerable<Voucher>> GetAllVouchersAsync();
        Task<Voucher> GetVoucherByIdAsync(int id);
        GenericResponse AddContraVoucherAsync(Voucher voucher);
        GenericResponse AddVoucherAsync(Voucher voucher, string serializedvoucherData);
        GenericResponse UpdateVoucher(Voucher voucher);
        Task DeleteVoucherAsync(int id);
    }
}
