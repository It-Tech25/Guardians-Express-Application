using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repository
{
    public class BankPaymentRepository : IBankPaymentRepository
    {
        private readonly MyDbContext _context;

        public BankPaymentRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<VoucherDto> GetBankPaymentRegisterDetails(int? branchId, string fromDate, string toDate)
        {
            var query = from voucher in _context.Vouchers
                        join branch in _context.branch on (string?)voucher.Branch equals branch.BranchName into branchGroup
                        from b in branchGroup.DefaultIfEmpty()
                        join voucherDetail in _context.voucherDetails on voucher.VoucherId equals voucherDetail.VoucherId into detailGroup
                        from vd in detailGroup.DefaultIfEmpty()
                        where voucher.VoucherType == "Bank Payment" // Filter only Cash Receipt vouchers
                        select new VoucherDto
                        {
                            VoucherId = voucher.VoucherId,
                            VoucherType = voucher.VoucherType,
                            Branch = b != null ? b.BranchName : string.Empty,
                            SerialNumber = voucher.SerialNumber,
                            VoucherNumber = voucher.VoucherNumber,
                            VoucherDate = voucher.VoucherDate,
                            AccountHead = voucher.AccountHead,
                            ChequeNumber = voucher.ChequeNumber,
                            ChequeDate = voucher.ChequeDate,
                            TotalAmount = voucher.TotalAmount,
                            IsDelete = voucher.IsDelete,
                            IsActive = voucher.IsActive,
                            CreatedOn = voucher.CreatedOn,
                            UpdatedOn = voucher.UpdatedOn,
                            DetailId = vd != null ? vd.DetailId : 0,
                            AccountDescription = vd != null ? vd.AccountDescription : string.Empty,
                            CurrentBalance = vd != null ? vd.CurrentBalance : 0,
                            Particular = vd != null ? vd.Particular : string.Empty,
                            BillToParty = vd != null ? vd.BillToParty : string.Empty,
                            BillNumber = vd != null ? vd.BillNumber : string.Empty,
                            VehicleNumber = vd != null ? vd.VehicleNumber : string.Empty,
                            TransactionType = vd != null ? vd.TransactionType : string.Empty,
                            Amount = vd != null ? vd.Amount : 0,
                            BalAmount = vd != null ? vd.BalAmount : 0,

                        };

            // Apply filters
            if (branchId.HasValue && branchId.Value > 0)
            {
                query = query.Where(x => x.VoucherId == branchId.Value);
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParse(fromDate, out DateTime parsedFromDate))
                {
                    query = query.Where(x => x.VoucherDate >= parsedFromDate);
                }
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParse(toDate, out DateTime parsedToDate))
                {
                    query = query.Where(x => x.VoucherDate <= parsedToDate);
                }
            }

            // Filter for active records only
            query = query.Where(x => x.IsActive == true && x.IsDelete == false);

            return query.OrderByDescending(x => x.VoucherDate).ToList();
        }
    }
}