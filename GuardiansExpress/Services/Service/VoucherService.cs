using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }
       public IEnumerable<Voucher> GetAllVouchers()
        {
            return _voucherRepository.GetAllVouchers();
        }
        public async Task<IEnumerable<Voucher>> GetAllVouchersAsync()
        {
            return await _voucherRepository.GetAllVouchersAsync();
        }

        public async Task<Voucher> GetVoucherByIdAsync(int id)
        {
            return await _voucherRepository.GetVoucherByIdAsync(id);
        }

        public GenericResponse AddVoucherAsync(Voucher voucher, string serializedvoucherData)
        {
           return _voucherRepository.AddVoucherAsync(voucher,serializedvoucherData);
        }
        public GenericResponse AddContraVoucherAsync(Voucher voucher)
        {
            return _voucherRepository.AddContraVoucherAsync(voucher);
        }


        public GenericResponse UpdateVoucherAsync(Voucher voucher)
        {
            return _voucherRepository.UpdateVoucher(voucher);
        }

        public async Task DeleteVoucherAsync(int id)
        {
            await _voucherRepository.DeleteVoucherAsync(id);
        }
    }
}
