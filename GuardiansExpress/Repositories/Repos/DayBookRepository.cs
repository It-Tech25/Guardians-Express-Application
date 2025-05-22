using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace GuardiansExpress.Repositories.Repos
{
    public class DayBookRepository : IDayBookRepository
    {
        private readonly MyDbContext _context;
        public DayBookRepository(MyDbContext context)
        {
            _context = context;
        }
        public List<DayBookDTO> GetDayBookEntries(DateTime startDate, DateTime endDate,
            string branch, string transactionType, string accHead, string balType, string bookType)
        {
            var query = _context.Vouchers
                .Include(v => v.VoucherDetail)
                .Where(v =>
                    v.IsDelete == false &&
                    v.IsActive == true &&
                    v.VoucherDate.Date >= startDate.Date &&
                    v.VoucherDate.Date <= endDate.Date)
                .AsQueryable();
            // Filter by Branch
            if (!string.IsNullOrWhiteSpace(branch) && branch != "All")
                query = query.Where(v => v.Branch == branch);
            // Filter by Transaction Type
            if (!string.IsNullOrWhiteSpace(transactionType) && transactionType != "All")
            {
                query = query.Where(v =>
                    v.VoucherType == transactionType ||
                    v.FromTransactionType == transactionType ||
                    v.ToTransactionType == transactionType);
            }
            // Filter by Account Head
            if (!string.IsNullOrWhiteSpace(accHead))
                query = query.Where(v => v.AccountHead != null && v.AccountHead.Contains(accHead));
            // Filter by Book Type
            if (!string.IsNullOrWhiteSpace(bookType))
                query = query.Where(v => v.VoucherType == bookType);

            // Project to DTO
            var data = query
                .SelectMany(v => v.VoucherDetail!.Select(d => new DayBookDTO
                {
                    Date = v.VoucherDate,
                    ReferenceNo = v.SerialNumber,
                    AccountHead = v.AccountHead,
                    Particulars = d.AccountDescription, // Add the description from voucher detail
                    VoucherNo = v.VoucherNumber,
                    ChequeNo = v.ChequeNumber,
                    TransactionType=v.FromTransactionType
                    // Add DebitAmount and CreditAmount from voucher detail
                   // DebitAmount = d.DebitAmount,
                   // CreditAmount = d.CreditAmount
                }))
                .ToList();

            // Filter by Balance Type (Dr or Cr)
            if (!string.IsNullOrWhiteSpace(balType))
            {
                balType = balType.Trim().ToLower();
                if (balType == "dr")
                    data = data.Where(d => d.DebitAmount > 0).ToList();
                else if (balType == "cr")
                    data = data.Where(d => d.CreditAmount > 0).ToList();
            }
            return data.OrderBy(d => d.Date).ToList();
        }
    }
}