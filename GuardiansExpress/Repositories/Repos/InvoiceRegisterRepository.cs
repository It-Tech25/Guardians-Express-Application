using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repository
{
    public class InvoiceRegisterRepository : IInvoiceRegisterRepository
    {
        private readonly MyDbContext _context;

        public InvoiceRegisterRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<InvoiceDTO> GetInvoiceRegisterDetails(int? branchId, string fromDate, string toDate)
        {
            var query = from invoice in _context.Invoices
                        join branch in _context.branch on invoice.BranchId equals branch.id into branchGroup
                        from b in branchGroup.DefaultIfEmpty()
                        select new InvoiceDTO
                        {
                            InvoiceId = invoice.InvoiceId,
                            BranchId = invoice.BranchId,
                            BranchName = b != null ? b.BranchName : string.Empty,
                            InvTypeId = invoice.InvTypeId,
                            InvoiceType = _context.invoice.Where(a=>a.Id==invoice.InvTypeId && a.IsDeleted==false).Select(a=>a.InvoiceType).FirstOrDefault(),
                            SNo = invoice.SNo,
                            InvoiceNo = invoice.InvoiceNo,
                            InvDate = invoice.InvDate,
                            GSTType = invoice.GSTType,
                            ClientId = invoice.ClientId,
                            SelectAddress = invoice.SelectAddress,
                            AccGSTIN = invoice.AccGSTIN,
                            Address = invoice.Address,
                            SelectContact = invoice.SelectContact,
                            ContactPerson = invoice.ContactPerson,
                            ClientEmail = invoice.ClientEmail,
                            ClientMobile = invoice.ClientMobile,
                            OrderNo = invoice.OrderNo,
                            OrderDate = invoice.OrderDate,
                            PONo = invoice.PONo,
                            PODate = invoice.PODate,
                            DueDate = invoice.DueDate,
                            ShipToSelectAddress = invoice.ShipToSelectAddress,
                            ShipToGSTIN = invoice.ShipToGSTIN,
                            ShipToAddress = invoice.ShipToAddress,
                            Mode = invoice.Mode,
                            VehicleNo = invoice.VehicleNo,
                            GREwayNo = invoice.GREwayNo,
                            GRDate = invoice.GRDate,
                            EwayBillNo = invoice.EwayBillNo,
                            Packages = invoice.Packages,
                            Transporter = invoice.Transporter,
                            TransporterId = invoice.TransporterId,
                            DispatchFromState = invoice.DispatchFromState,
                            StateName = _context.stateEntities.Where(a=>a.Id==invoice.DispatchFromState).Select(a=>a.StateName).FirstOrDefault(),
                            DispatchFromCity = invoice.DispatchFromCity,
                            DispatchFromPincode = invoice.DispatchFromPincode,
                            DispatchFrom = invoice.DispatchFrom,
                            CostCenter = invoice.CostCenter,
                            ChallanNo = invoice.ChallanNo,
                            PaymentTerm = invoice.PaymentTerm,
                            GrossAmount = invoice.GrossAmount,
                            Discount = invoice.Discount,
                            Tax = invoice.Tax,
                            RoundOff = invoice.RoundOff,
                            NetAmount = invoice.NetAmount,
                            IsActive = invoice.IsActive,
                            IsDeleted = invoice.IsDeleted,
                            CreatedOn = invoice.CreatedOn,
                            CreatedBy = invoice.CreatedBy,
                            UpdatedOn = invoice.UpdatedOn,
                            UpdatedBy = invoice.UpdatedBy
                        };

            // Apply filters
            if (branchId.HasValue && branchId.Value > 0)
            {
                query = query.Where(x => x.BranchId == branchId.Value);
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParse(fromDate, out DateTime parsedFromDate))
                {
                    query = query.Where(x => x.InvDate >= parsedFromDate);
                }
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParse(toDate, out DateTime parsedToDate))
                {
                    query = query.Where(x => x.InvDate <= parsedToDate);
                }
            }

            // Filter for active records only
            query = query.Where(x => x.IsActive == true && x.IsDeleted == false);

            return query.OrderByDescending(x => x.InvDate).ToList();
        }
    }
}