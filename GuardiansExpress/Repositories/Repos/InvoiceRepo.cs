using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly MyDbContext _context;

        public InvoiceRepo(MyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<InvoiceDTO> GetInvoice(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from Invoice in _context.Invoices
                        join branch in _context.branch on Invoice.BranchId equals branch.id
                        join i in _context.invoice on Invoice.InvTypeId equals i.Id
                        join s in _context.stateEntities on Invoice.DispatchFromState equals s.Id
                        where Invoice.IsDeleted == false
                        select new InvoiceDTO
                        {
                            InvoiceId = Invoice.InvoiceId,
                            BranchId = Invoice.BranchId,
                            BranchName = branch.BranchName,
                            InvoiceType = i.InvoiceType,
                            InvTypeId = Invoice.InvTypeId,
                            SNo = Invoice.SNo,
                            InvoiceNo = Invoice.InvoiceNo,
                            InvDate = Invoice.InvDate,
                            GSTType = Invoice.GSTType,
                            ClientId = Invoice.ClientId,
                            SelectAddress = Invoice.SelectAddress,
                            AccGSTIN = Invoice.AccGSTIN,
                            Address = Invoice.Address,
                            SelectContact = Invoice.SelectContact,
                            ContactPerson = Invoice.ContactPerson,
                            ClientEmail = Invoice.ClientEmail,
                            ClientMobile = Invoice.ClientMobile,
                            OrderNo = Invoice.OrderNo,
                            PONo = Invoice.PONo,
                            PODate = Invoice.PODate,
                            DueDate = Invoice.DueDate,
                            ShipToSelectAddress = Invoice.ShipToSelectAddress,
                            ShipToGSTIN = Invoice.ShipToGSTIN,
                            ShipToAddress = Invoice.ShipToAddress,
                            Mode = Invoice.Mode,
                            NetAmount = Invoice.NetAmount,
                            GrossAmount = Invoice.GrossAmount,
                            VehicleNo = Invoice.VehicleNo,
                            GREwayNo = Invoice.GREwayNo,
                            GRDate = Invoice.GRDate,
                            EwayBillNo = Invoice.EwayBillNo,
                            Packages = Invoice.Packages,
                            Transporter = Invoice.Transporter,
                            TransporterId = Invoice.TransporterId,
                            DispatchFromState = Invoice.DispatchFromState,
                            DispatchFromCity = Invoice.DispatchFromCity,
                            DispatchFromPincode = Invoice.DispatchFromPincode,
                            DispatchFrom = Invoice.DispatchFrom,
                            CostCenter = Invoice.CostCenter,
                            ChallanNo = Invoice.ChallanNo,
                            PaymentTerm = Invoice.PaymentTerm,
                            StateName = s.StateName,

                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {

                query = query.Where(v => v.ContactPerson.Contains(searchTerm));
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }
        public InvoiceEntity GetInvoiceById(int id)
        {
            return _context.Invoices.Find(id);
        }
        public GenericResponse createInvoice(InvoiceEntity Invoice)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                Invoice.IsDeleted = false;
                Invoice.IsActive = true;
                _context.Invoices.Add(Invoice);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Invoice created successfully";
                response.currentId = Invoice.InvoiceId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Invoice: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
        public GenericResponse UpdateInvoice(InvoiceEntity Invoice)
        {
            GenericResponse response = new GenericResponse();
            try
            {

                var existing = _context.Invoices.Where(c => c.InvoiceId == Invoice.InvoiceId).FirstOrDefault();
                if (existing != null)
                {
                    Invoice.IsActive = true;
                    Invoice.IsDeleted = false;
                    Invoice.UpdatedOn = DateTime.Now;


                    _context.SaveChanges();

                    _context.Entry(existing).CurrentValues.SetValues(Invoice);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Invoice updated successfully";
                    response.currentId = Invoice.InvoiceId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Invoice not found";
                    response.currentId = 0;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Invoice: " + ex.Message;
                response.currentId = 0;
                return response;
            }

        }
        public GenericResponse DeleteInvoice(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.Invoices.Where(v => v.InvoiceId == id).FirstOrDefault();
                if (existing != null)
                {

                    existing.IsDeleted = true;
                    _context.Update(existing);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = " Invoice deleted successfully";
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
                response.message = "Failed to delete Invoice: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

    }
}

