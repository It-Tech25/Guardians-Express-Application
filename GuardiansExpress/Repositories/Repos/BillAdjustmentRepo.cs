using System.IO;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
    public class BillAdjustmentRepo :IBillAdjustmentRepo
    {
        private readonly MyDbContext _context;

        public BillAdjustmentRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BillAdjustmentDTO> GetBillAdjustment(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from Bill in _context.BillAdjustments

                        where Bill.IsDeleted == false
                        select new BillAdjustmentDTO
                        {
                            BalanceId = Bill.BalanceId,
                            BalanceBills= Bill.BalanceBills,
                           
                            UnderGroup= Bill.UnderGroup,
                            Party= Bill.Party,
                            VoucherNumber= Bill.VoucherNumber,
                            VoucherDate= Bill.VoucherDate,
                            BillNumber=Bill.BillNumber,
                            Bill_Date=Bill.Bill_Date,
                            BillAmt = Bill.BillAmt,
                            BalAmt= Bill.BalAmt,
                            RefDescription= Bill.RefDescription,
                            Particular= Bill.Particular,
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.Party.Contains(searchTerm) ||
                                         v.Particular.Contains(searchTerm) ||
                                         v.RefDescription.Contains(searchTerm)

                                         );
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public BillAdjustmentEntity GetBillAdjustmentById(int id)
        {

            return _context.BillAdjustments.Find(id);
        }
        public GenericResponse CreateBillAdjustment(BillAdjustmentEntity Bill)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                Bill.IsDeleted = false;
                Bill.IsActive = true;
                _context.BillAdjustments.Add(Bill);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "BillAdjustment created successfully";
                response.currentId = Bill.BalanceId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save BillAdjustment : " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
        public GenericResponse UpdateBillAdjustment(BillAdjustmentEntity Bill)
        {
            GenericResponse response = new GenericResponse();
            try
            {

                var existing = _context.BillAdjustments.Where(b => b.BalanceId == Bill.BalanceId).FirstOrDefault();
                if (existing != null)
                {
                    Bill.IsActive = true;
                    Bill.IsDeleted = false;

                    Bill.UpdatedOn = DateTime.Now;


                    _context.SaveChanges();

                    _context.Entry(existing).CurrentValues.SetValues(Bill);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "BillAdjustment updated successfully";
                    response.currentId = Bill.BalanceId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "BillAdjustment not found";
                    response.currentId = 0;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update BillAdjustment: " + ex.Message;
                response.currentId = 0;
                return response;
            }

        }
        public GenericResponse DeleteBillAdjustment(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {

                var existing = _context.BillAdjustments.Where(b => b.BalanceId == id).FirstOrDefault();
                if (existing != null)
                {

                    existing.IsDeleted = true;
                    _context.Update(existing);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = " BillAdjustment deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete BillAdjustment: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
