using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IVoucherService
    {
        IEnumerable<Voucher> GetAllVouchers();
        Task<IEnumerable<Voucher>> GetAllVouchersAsync();
        Task<Voucher> GetVoucherByIdAsync(int id);
        GenericResponse AddVoucherAsync(Voucher voucher, string serializedvoucherData);
        GenericResponse AddContraVoucherAsync(Voucher voucher);
        GenericResponse UpdateVoucherAsync(Voucher voucher);
        Task DeleteVoucherAsync(int id);
    }
}
