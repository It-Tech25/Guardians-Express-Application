using GuardiansExpress.Models.DTOs; 
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GuardiansExpress.Repositories.Repos
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly MyDbContext _context;

        public VoucherRepository(MyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Voucher> GetAllVouchers()
        {
            var res = _context.Vouchers.ToList();
            return res; 
        }
        public async Task<IEnumerable<Voucher>> GetAllVouchersAsync()
        {
            return await _context.Set<Voucher>().Include(v => v.VoucherDetail).ToListAsync();
        }

        public async Task<Voucher> GetVoucherByIdAsync(int id)
        {
            return await _context.Set<Voucher>().Include(v => v.VoucherDetail)
                .FirstOrDefaultAsync(v => v.VoucherId == id);
        }

        public GenericResponse AddVoucherAsync(Voucher voucher, string serializedvoucherData=null)
        {
            var vdetails = JsonConvert.DeserializeObject<List<VoucherDetail>>(serializedvoucherData);
            try { 
                voucher.IsDelete = false;
                voucher.IsActive = true;
                voucher.CreatedOn = DateTime.UtcNow;

                _context.Vouchers.Add(voucher);
                _context.SaveChanges(); 

                if (vdetails != null && vdetails.Any())
                {
                    foreach (var detail in vdetails)
                    {
                        detail.VoucherId = voucher.VoucherId; 
                    }

                    _context.voucherDetails.AddRange(vdetails); 
                    _context.SaveChanges();



                }

                return new GenericResponse { statuCode = 1, message = "Voucher and details added successfully." };
            }
            catch (Exception ex)
            {
                return new GenericResponse { statuCode = 0, message = "An error occurred while adding the voucher." };
            }


        }
        public GenericResponse AddContraVoucherAsync(Voucher voucher)
        {
            var frombank = _context.ledgerEntity.Where(a => a.AccHead == voucher.FromBank && a.AccGroup == "Bank Accounts").FirstOrDefault();
            var tobank = _context.ledgerEntity.Where(a => a.AccHead == voucher.ToBank && a.AccGroup == "Bank Accounts").FirstOrDefault();
            Voucher v = new Voucher();
          // var vdetails = JsonConvert.DeserializeObject<List<VoucherDetail>>(serializedvoucherData);
            try
            {
                v.VoucherType = voucher.VoucherType;
                v.FromBank = voucher.FromBank;
                v.ToBank = voucher.ToBank;
                v.BalanceAmount = voucher.BalanceAmount;
                v.ToBalanceAmount = voucher.ToBalanceAmount;
                v.TransferAmount = voucher.TransferAmount;
                v.FromTransactionType = voucher.FromTransactionType;
                v.ReceiveAmount = voucher.ReceiveAmount;
                v.ToTransactionType = voucher.ToTransactionType;
                v.IsDelete = false;
                v.IsActive = true;
                v.CreatedOn = DateTime.UtcNow;

                _context.Vouchers.Add(v);

                _context.SaveChanges();

                frombank.BalanceOpening = frombank.BalanceOpening - v.TransferAmount;
                _context.ledgerEntity.Update(frombank);
                _context.SaveChanges();
                tobank.BalanceOpening = tobank.BalanceOpening + v.ReceiveAmount;
                _context.ledgerEntity.Update(tobank);
                _context.SaveChanges();
                return new GenericResponse { statuCode = 1, message = "Voucher  added successfully." };
            }
            catch (Exception ex)
            {
                return new GenericResponse { statuCode = 0, message = "An error occurred while adding the voucher." };
            }


        }

        public GenericResponse UpdateVoucher(Voucher voucher)
        {
            try
            {
                var frombank = _context.ledgerEntity.Where(a => a.AccHead == voucher.FromBank && a.AccGroup == "Bank Accounts").FirstOrDefault();
                var tobank = _context.ledgerEntity.Where(a => a.AccHead == voucher.ToBank && a.AccGroup == "Bank Accounts").FirstOrDefault();

                voucher.UpdatedOn = DateTime.UtcNow;
                _context.Vouchers.Update(voucher);

                if (voucher.VoucherDetail != null && voucher.VoucherDetail.Any())
                {
                    var existingDetails =_context.voucherDetails
                        .Where(vd => vd.VoucherId == voucher.VoucherId)
                        .ToList();

                    foreach (var detail in voucher.VoucherDetail)
                    {
                            _context.voucherDetails.Update(detail);
                      
                    }
                }

               _context.SaveChanges();
                frombank.BalanceOpening = frombank.BalanceOpening - voucher.TransferAmount;
                _context.ledgerEntity.Update(frombank);
                _context.SaveChanges();
                tobank.BalanceOpening = tobank.BalanceOpening + voucher.ReceiveAmount;
                _context.ledgerEntity.Update(tobank);
                _context.SaveChanges();
                return new GenericResponse { statuCode = 1, message = "Voucher and details Updated successfully." };
            }
            catch (Exception ex)
            {
               return new GenericResponse { statuCode = 0, message = "An error occurred while Update the voucher." };
            }
        }

        public async Task DeleteVoucherAsync(int id)
        {
            var voucher = await GetVoucherByIdAsync(id);
            if (voucher != null)
            {
                _context.Set<Voucher>().Remove(voucher);
                await _context.SaveChangesAsync();
            }
        }
    }
}
