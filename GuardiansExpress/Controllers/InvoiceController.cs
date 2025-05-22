
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Services.Service;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class InvoiceController : Controller
    {

        private readonly IBranchMasterService _bservice;
        private readonly IInvoiceTypeService _iiservice;
        private readonly ITaxMasterService _tservice;

        private readonly IInvoiceService _iservice;
        private readonly ICountryAndStateService _tsservice;

        private readonly MyDbContext _context;

        public InvoiceController(ICountryAndStateService tsservice, IInvoiceService iservice, ITaxMasterService tservice, IInvoiceTypeService iiservice, MyDbContext context, IBranchMasterService bservice)
        {
            _bservice = bservice;
            _iiservice = iiservice;
            _context = context;
            _iservice = iservice;
            _tservice = tservice;
            _tsservice = tsservice;
        }


        public IActionResult InvoiceIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var client = _context.ledgerEntity.Where(a => a.IsDeleted == false).Select(a => a.AccHead).ToList();
            ViewBag.ClientName = client;
            // Pass user details to the view
            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;
            //ViewBag.UserProfileImage = loggedInUser.ProfileImageUrl; // URL to the user's profile image
            var query = _context.Invoices.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.ContactPerson.Contains(searchTerm));
            }

            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var vehicleEntities = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var Invoices = _iservice.GetInvoices(searchTerm, pageNumber, pageSize);

            var InvoiceDTOs = Invoices.Select(i => new InvoiceDTO
            {
                InvoiceId = i.InvoiceId,
                BranchId = i.BranchId,
                InvTypeId = i.InvTypeId,
                SNo = i.SNo,
                InvoiceNo = i.InvoiceNo,
                InvDate = i.InvDate,
                GSTType = i.GSTType,
                ClientId = i.ClientId,
                SelectAddress = i.SelectAddress,
                AccGSTIN = i.AccGSTIN,
                Address = i.Address,
                SelectContact = i.SelectContact,
                ContactPerson = i.ContactPerson,
                ClientEmail = i.ClientEmail,
                ClientMobile = i.ClientMobile,
                NetAmount = i.NetAmount,
                GrossAmount = i.GrossAmount,
                OrderNo = i.OrderNo,
                OrderDate = i.OrderDate,
                PONo = i.PONo,
                PODate = i.PODate,
                DueDate = i.DueDate,
                ShipToSelectAddress = i.ShipToSelectAddress,
                ShipToGSTIN = i.ShipToGSTIN,
                ShipToAddress = i.ShipToAddress,
                Mode = i.Mode,
                VehicleNo = i.VehicleNo,
                GREwayNo = i.GREwayNo,
                GRDate = i.GRDate,
                EwayBillNo = i.EwayBillNo,
                Packages = i.Packages,
                Transporter = i.Transporter,
                TransporterId = i.TransporterId,
                DispatchFromState = i.DispatchFromState,
                DispatchFromCity = i.DispatchFromCity,
                DispatchFromPincode = i.DispatchFromPincode,
                DispatchFrom = i.DispatchFrom,
                CostCenter = i.CostCenter,
                ChallanNo = i.ChallanNo,
                PaymentTerm = i.PaymentTerm,
                IsDeleted = i.IsDeleted,
                BranchName = i.BranchName,
                InvoiceType = i.InvoiceType,
                IsActive = i.IsActive,
                StateName = i.StateName,

            }).ToList();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;
            var i = _iservice.GetInvoices(searchTerm, pageNumber, pageSize).ToList();
            var b = _bservice.BranchMaster(searchTerm, pageNumber, pageSize).ToList();
            var v = _iiservice.GetInvoiceTypes().ToList();
            var s = _tservice.GetTaxMastersWithLedger(searchTerm, pageNumber, pageSize).ToList();
            var st = _tsservice.StatebyContry().ToList();
            ViewBag.i = i;
            ViewBag.b = b;
            ViewBag.v = v;
            ViewBag.s = s;
            ViewBag.st = st;
            return View(InvoiceDTOs);
        }




        [HttpPost]
        public JsonResult createInvoice(InvoiceDTO model)
        {
            try
            {
                var invoice = new InvoiceEntity
                {
                    InvoiceId = model.InvoiceId,
                    BranchId = model.BranchId,
                    InvTypeId = model.InvTypeId,
                    SNo = model.SNo,
                    InvoiceNo = model.InvoiceNo,
                    GSTType = model.GSTType,
                    ClientId = model.ClientId,
                    SelectAddress = model.SelectAddress,
                    AccGSTIN = model.AccGSTIN,
                    Address = model.Address,
                    SelectContact = model.SelectContact,
                    ContactPerson = model.ContactPerson,
                    ClientEmail = model.ClientEmail,
                    ClientMobile = model.ClientMobile,
                    OrderNo = model.OrderNo,
                    OrderDate = model.OrderDate,
                    PONo = model.PONo,
                    PODate = model.PODate,
                    DueDate = model.DueDate,
                    ShipToSelectAddress = model.ShipToSelectAddress,
                    ShipToGSTIN = model.ShipToGSTIN,
                    ShipToAddress = model.ShipToAddress,
                    Mode = model.Mode,
                    VehicleNo = model.VehicleNo,
                    GREwayNo = model.GREwayNo,
                    GRDate = model.GRDate,
                    EwayBillNo = model.EwayBillNo,
                    Packages = model.Packages,
                    Transporter = model.Transporter,
                    TransporterId = model.TransporterId,
                    DispatchFromState = model.DispatchFromState,
                    DispatchFromCity = model.DispatchFromCity,
                    DispatchFromPincode = model.DispatchFromPincode,
                    DispatchFrom = model.DispatchFrom,
                    CostCenter = model.CostCenter,
                    ChallanNo = model.ChallanNo,
                    PaymentTerm = model.PaymentTerm,
                    IsDeleted = false,
                    IsActive = true
                };

                _context.Invoices.Add(invoice);
                _context.SaveChanges(); // Save to get ID


                if (model.BillItems != null && model.BillItems.Any())
                {
                    foreach (var item in model.BillItems)
                    {
                        var billItem = new salesdetailsEntity
                        {
                            InvoiceId = invoice.InvoiceId,
                            ItemId = item.ItemId,
                            GRNo = item.GRNo,
                            GRDate = item.GRDate,
                            HSCSSN = item.HSCSSN,
                            DetentionAmount = item.DetentionAmount,
                            DetentionDays = item.DetentionDays,
                            Rate = item.Rate,
                            OtherCharges = item.OtherCharges,
                            Remarks = item.Remarks,
                            Amount = item.Amount,
                            TaxId = item.TaxId,
                            Tax_Amt = item.Tax_Amt,
                            TotalAmount = item.TotalAmount,
                            Total = item.Total,
                            IsActive = item.IsActive ?? true,
                            IsDeleted = item.IsDeleted ?? false,
                            CreatedOn = item.CreatedOn ?? DateTime.Now,
                            CreatedBy = item.CreatedBy
                        };

                        _context.salesdetailsEntitys.Add(billItem);
                    }

                    _context.SaveChanges(); // Save Bill Items
                }

                return Json(new { success = true, message = "Invoice added successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error saving data: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateInvoice(InvoiceEntity model)
        {
            var response = _iservice.UpdateInvoice(model);
            if (response.statuCode == 1)
            {
                return RedirectToAction("InvoiceIndex");
            }
            return Json(new { success = false, message = response.message });
        }
        [HttpPost]
        public IActionResult DeleteInvoice(int id)
        {

            if (id == 0)
            {
                return BadRequest("Invalid Invoice ID.");
            }


            var response = _iservice.DeleteInvoice(id);

            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        public IActionResult GrRecords(string clientName)
        {
            var records = from gr in _context.grDetails
                          where (gr.ClientName == clientName)

                          select new GRDTOs
                          {
                              GRId = gr.GRId,
                              Branch = gr.Branch,
                              VehicleNo = gr.VehicleNo,
                              OwnedBy = gr.OwnedBy,
                              Grtype = gr.Grtype,
                              GRNo = gr.GRNo,
                              GRDate = gr.GRDate,
                              Consigner = gr.Consigner,
                              Consignee = gr.Consignee,
                              ClientName = gr.ClientName,
                              Transporter = gr.Transporter,
                              StatesFromPlace = _context.placeEntity.Where(a => a.Id == gr.FromPlace).Select(a => a.PlaceName).FirstOrDefault(),
                              States = _context.placeEntity.Where(a => a.Id == gr.ToPlace).Select(a => a.PlaceName).FirstOrDefault(),
                              ItemDescription = gr.ItemDescription,
                              FreightAmount = gr.FreightAmount,
                              GrossWeight = gr.GrossWeight,
                              LoadWeight = gr.LoadWeight,
                              HireRate = gr.HireRate,
                              Quantity = gr.Quantity,
                              //InvoiceDate = gr.InvoiceDate,
                              //EWayBillNo = gr.EWayBillNo,
                              //Refno = gr.Refno,
                              //RefDate = gr.RefDate,
                              //RecRefNo = gr.RecRefNo,
                          };

            return Json(records);

        }
        public IActionResult GetClients()
        {
            var clients = _context.ledgerEntity.Where(a => a.IsDeleted == false).Select(a => a.AccHead).ToList();  // however you get your data
            return Json(clients);
        }
        [HttpGet]
        public JsonResult GetHSCSSNList()
        {
            var list = _context.CompanyConfigurations
                               .Select(c => new {
                                   Value = c.HSCSSN,
                                   Text = c.HSCSSN
                               })
                               .ToList();

            return Json(list);
        }

    }
}