using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Services.Service;
using OfficeOpenXml; // For EPPlus library
//using ClosedXML.Excel; // For Excel export
using iTextSharp.text; // For PDF export
using iTextSharp.text.pdf;
using GuardiansExpress.Utilities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace GuardiansExpress.Controllers
{
    public class GRController : Controller
    {
        private readonly IGRService _grService;
        private readonly IBranchMasterService _brService;
        private readonly IGRTypeService _grTypeService;
        private readonly IVehicleMasterService _service;
        private readonly IBranchMasterService _bservice;
        private readonly IVehicleTypeService _vservice;
        private readonly IBodyTypeService _btservice;
        private readonly MyDbContext _context;
        public GRController(IGRService grService, IBranchMasterService brService, IGRTypeService grTypeService, IBranchMasterService bservice, IVehicleTypeService vservice, IBodyTypeService btservice, MyDbContext context)
        {
            _grService = grService;
            _brService = brService;
            _grTypeService = grTypeService;
            _bservice = bservice;
            _vservice = vservice;
            _btservice = btservice;
            _context = context;
        }
        public IEnumerable<GRDTOs> Getgrdetails()
        {
            var res = _grService.Getgrdetails();
            return res;
        }
        public IActionResult GRIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            //      var lastGrNumbersBySeries = _context.grnoEntity
            //.Where(a => a.IsDeleted == false && a.GR != null)
            //.AsEnumerable() // move to LINQ-to-Objects for regex parsing
            //.Select(a => new
            //{
            //    Series = System.Text.RegularExpressions.Regex.Match(a.GR, @"^[A-Za-z]+").Value,
            //    Number = int.TryParse(System.Text.RegularExpressions.Regex.Match(a.GR, @"\d+$").Value, out int num) ? num : 0,
            //    FullGR = a.GR
            //})
            //.GroupBy(g => g.Series)
            //.Select(g => new
            //{
            //    Series = g.Key,
            //    LastGR = g.OrderByDescending(x => x.Number).First().FullGR
            //})
            //.ToList();
            List<SeriesGR> data = _context.grnoEntity
          .Where(x => x.IsDeleted==false && x.GR != null)
          .GroupBy(x => x.GR)
          .Select(g => new SeriesGR
          {
              Series = g.Key,
              LastGR = g.OrderByDescending(x => x.GR).FirstOrDefault().GR
          }).ToList();

            ViewBag.LastGRBySeries = data;
           
            // Pass user details to the view
            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;
            //ViewBag.UserProfileImage = loggedInUser.ProfileImageUrl; // URL to the user's profile image
            var res = Getgrdetails();

            var b = _bservice.BranchMaster(searchTerm, pageNumber, pageSize).ToList();
            var v = _vservice.GetVehicleTypes(searchTerm, pageNumber, pageSize).ToList();



            var bt = _btservice.GetBodyTypes().ToList();

            ViewBag.b = b;
            ViewBag.v = v;
            ViewBag.bt = bt;

            ViewBag.Branch = _brService.BranchMaster(searchTerm, pageNumber, pageSize);
            ViewBag.Grtype = _grTypeService.GetAllGRTypes();

            var vno = _context.VehicleMasters
                    .Where(a => a.IsDeleted == false && a.IsActive == true)
                    .Select(a => a.VehicleNo)  // Assuming 'VehicleNo' is the field you want to display
                    .Distinct().ToList();

            ViewBag.VehicleNo = vno;
            return View(res);
        }

        [HttpPost]
        public IActionResult GRAdd(GRDTOs gr, string serializedinvoiceData)
        {
            var response = _grService.AddAsync(gr, serializedinvoiceData);
            
                if (response.statuCode == 1)
                {   return Json(new { success = true, grId = response.currentId });


                }
                else
                {
                    return RedirectToAction("GRIndex");
                }
            
           
        }


        [HttpPost]
        public IActionResult UpdateGR(GRDTOs grDto, string serializedInvoiceData)
        {
            try
            {
                var res = _grService.UpdateAsync(grDto, serializedInvoiceData);

                if (res.statuCode == 1)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, grId = res.currentId });
                    }
                    return RedirectToAction("GRIndex");
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                return Json(new { success = false, message = ex.Message });
            }
        }
    
        [Route("GR/Print/{grId}")]
        public IActionResult Print(int grId)
        {
            var gr = _context.grDetails.Where(a => a.GRId == grId & a.IsDeleted == false).FirstOrDefault();
            var grdetails = _context.invoicegr.Where(a => a.GRId == gr.GRId).FirstOrDefault();
            var vehicletype = _context.VehicleMasters.Where(a => a.VehicleNo == gr.VehicleNo).Select(a => a.VehicleTypeId).FirstOrDefault();
            if (gr == null)
            {
                return NotFound();
            }
            ConsignmentNote note = new ConsignmentNote();
            note.LrNumber = gr.GRNo.ToString();
            note.LrDate = _context.VehicleMasters.Where(a => a.VehicleNo == gr.VehicleNo).Select(a => a.LRDate).FirstOrDefault();
            note.TruckNumber = gr.VehicleNo;
            note.TruckType = _context.VehicleTypeEntite.Where(a => a.Id == vehicletype).Select(a => a.VehicleType).FirstOrDefault();
            note.ConsignorName = gr.Consigner;
            note.ConsignorAddress = _context.AddressBookMaster.Where(a => a.ContactPersonName == gr.Consigner).Select(a => a.Address).FirstOrDefault();
            note.ConsignorState = _context.placeEntity.Where(a => a.Id == gr.FromPlace).Select(a => a.PlaceName).FirstOrDefault();
            note.ConsignorGSTIN = _context.CompanyConfigurations.Where(a => a.Name == gr.Consigner).Select(a => a.GSTIN).FirstOrDefault();
            note.ConsigneeName = gr.Consignee;
            note.ConsigneeAddress = _context.AddressBookMaster.Where(a => a.ContactPersonName == gr.Consignee).Select(a => a.Address).FirstOrDefault();
            note.ConsigneeState = _context.placeEntity.Where(a => a.Id == gr.ToPlace).Select(a => a.PlaceName).FirstOrDefault();
            note.ConsigneeGSTIN = _context.CompanyConfigurations.Where(a => a.Name == gr.Consignee).Select(a => a.GSTIN).FirstOrDefault();
            note.NoOfPackages = gr.Quantity;
            note.GrossWeight = gr.GrossWeight;
            note.LoadWeight = gr.LoadWeight;
            note.Description = gr.ItemDescription;
            note.InvoiceNumber = grdetails.InvoiceNo;
            note.InvoiceDate = grdetails.InvoiceDate.ToString();
            note.InvoiceValue = grdetails.InvoiceValue;
            note.EWayBillNumber = grdetails.EwayBillNo;

           
            return View(note);
        }

        [HttpPost]
        public IActionResult DeleteGRs(int id)
        {
            var response = _grService.DeleteAsync(id);

            if (response.statuCode == 1)
            {
                return RedirectToAction("GRIndex");
            }
            else
            {
                return RedirectToAction("GRIndex");
            }

        }
        [HttpPost]
        public JsonResult GetClientNames(string term)
        {
            // Fetch the client names from your database that match the input 'term'
            var clientNames = _context.AddressBookMaster
                                      .Where(c => c.ClientName.Contains(term)) // Filtering based on search term
                                      .Select(c => c.ClientName)
                                      .Take(10) // Limit the results (optional)
                                      .ToList();

            return Json(clientNames);
        }


        //public JsonResult GetTransporterName(string vehicleNo)
        //{
        //    // Here, we query the database or service to fetch the transporter name based on vehicleNo
        //    var transporterName = GetTransporterByVehicleNo(vehicleNo);

        //    // Return transporter name as JSON response
        //    if (!string.IsNullOrEmpty(transporterName))
        //    {
        //        return Json(new { transporterName = transporterName });
        //    }
        //    else
        //    {
        //        return Json(new { transporterName = (string)null });
        //    }
        //}


        // Example method to simulate fetching transporter name from a database
        //private string GetTransporterByVehicleNo(string vehicleNo)
        //{
        //    // Replace with your actual database logic
        //    var transporter = _context.VehicleMasters.Where(a => a.VehicleNo == vehicleNo).Select(a => a.Transporter).FirstOrDefault();

        //    return transporter;
        //}

        public JsonResult GetStatesByClientName(string clientName)
        {
            // Query to get the states related to the clientName from the database
            var states = _context.AddressBookMaster
                                 .Where(c => c.ClientName == clientName)  // Adjust query to fetch states related to client
                                 .Select(c => c.State)  // Assuming States is a navigation property
                                 .Distinct()
                                 .ToList();

            return Json(states);  // Return states as JSON
        }


        public JsonResult GetStatesByConsigner(string consignerName)
        {
            // Query to get states related to the consigner (adjust the query as per your data model)
            var states = _context.AddressBookMaster
                                 .Where(c => c.ClientName == consignerName)  // Adjust to match your model
                                 .Select(c => c.State)  // Assuming States is a navigation property
                                 .Distinct()
                                 .ToList();

            return Json(states);  // Return the states as a JSON array
        }
        [HttpGet]
        public IActionResult GetTransporterByVehicleNo(string vehicleNo)
        {
            var transporter = _context.VehicleMasters
                                .Where(v => v.VehicleNo == vehicleNo)
                                .Select(v => v.Transporter)
                                .FirstOrDefault();

            return Json(new { transporter = transporter });
        }



    }
}
