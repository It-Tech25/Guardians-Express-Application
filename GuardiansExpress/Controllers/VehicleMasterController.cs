using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class VehicleMasterController : Controller
    {
        private readonly IVehicleMasterService _service;
        private readonly IBranchMasterService _bservice;
        private readonly IVehicleTypeService _vservice;
        private readonly IBodyTypeService _btservice;

        private readonly MyDbContext _context;

        public VehicleMasterController(IVehicleMasterService service, MyDbContext context, IBranchMasterService bservice, IVehicleTypeService vservice, IBodyTypeService btservice)
        {
            _bservice = bservice;
            _service = service;
            _vservice = vservice;
            _context = context;
            _btservice = btservice;
        }


        public IActionResult VehicleMaterIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            // Pass user details to the view
            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;
            //ViewBag.UserProfileImage = loggedInUser.ProfileImageUrl; // URL to the user's profile image
            var query = _context.VehicleMasters.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.VehicleNo.Contains(searchTerm));
            }

            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var vehicleEntities = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var vehicle = _service.GetVehicleMaster(searchTerm, pageNumber, pageSize);

            var vehicleDtos = vehicle.Select(v => new VehicleMasterDTO
            {
                VehicleId = v.VehicleId,
                VehicleNo = v.VehicleNo,
                BranchId = v.BranchId,
                BranchName = v.BranchName,
                VehicleTypeId = v.VehicleTypeId,
                BodyTypeId = v.BodyTypeId,
                DisplayVehicleNo = v.DisplayVehicleNo,
                Weight = v.Weight,
                OwnedBy = v.OwnedBy,
                Transporter = v.Transporter,
                MaxWeightAllowed = v.MaxWeightAllowed,
                Status = v.Status,
                DocumentType = v.DocumentType,
                StartDate = v.StartDate,
                ExpiryDate = v.ExpiryDate,
                Amount = v.Amount,
                Remarks = v.Remarks,
                Uploads = v.Uploads,
                Docs = v.Docs,
                IsActive = v.IsActive,
                IsDeleted = v.IsDeleted,
                CreatedOn = v.CreatedOn,
                CreatedBy = v.CreatedBy,
                UpdatedOn = v.UpdatedOn,
                UpdatedBy = v.UpdatedBy,

            }).ToList();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;
            var s = _service.GetVehicleMaster(searchTerm, pageNumber, pageSize).ToList();

            var b = _bservice.BranchMaster(searchTerm, pageNumber, pageSize).ToList();
            var v = _vservice.GetVehicleTypes(searchTerm, pageNumber, pageSize).ToList();


            var bt = _btservice.GetBodyTypes().ToList();

            ViewBag.b = b;
            ViewBag.s = s;
            ViewBag.v = v;
            ViewBag.bt = bt;

            return View(vehicleDtos);
        }

        

        [HttpPost]
        public IActionResult CreateVehicleMaster(
     VehicleMasterDTO model, 
     List<string> DocumentType,
     List<DateTime> StartDate,
     List<DateTime> ExpiryDate,
     List<decimal> Amount,
     List<string> Remarks,
     List<IFormFile>? Uploads,
     List<string> Docs
 )
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            try
            {
                List<string> uploadedFilePaths = new List<string>(); 

                if (Uploads != null && Uploads.Count > 0)
                {
                    foreach (var file in Uploads) 
                    {
                        if (file.Length > 0)
                        {
                            var vehicleFileName = Path.GetFileName(file.FileName); 
                            var vehicleFilePath = Path.Combine("wwwroot/uploads", vehicleFileName); 

                            
                            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads")))
                            {
                                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads"));
                            }

                           
                            using (var stream = new FileStream(vehicleFilePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                           
                            var relativePath = $"/uploads/{vehicleFileName}";
                            uploadedFilePaths.Add(relativePath); 
                        }
                    }
                }

               
                string vehicleUploadPath = string.Join(",", uploadedFilePaths);

               
                var vehicle = new VehicleMasterEntity
                {
                    VehicleId = model.VehicleId,
                    VehicleNo = model.VehicleNo,
                    DisplayVehicleNo = model.DisplayVehicleNo,
                    Weight = model.Weight,
                    OwnedBy = model.OwnedBy,
                    Transporter = model.Transporter,
                    MaxWeightAllowed = model.MaxWeightAllowed,
                    Status = model.Status,
                    StartDate = model.StartDate,
                    DocumentType = model.DocumentType,
                    ExpiryDate = model.ExpiryDate,
                    BranchId = model.BranchId,
                    VehicleTypeId = model.VehicleTypeId,
                    BodyTypeId = model.BodyTypeId,
                    Amount = model.Amount,
                    Remarks = model.Remarks,
                    Uploads = vehicleUploadPath, 
                    Docs = model.Docs,
                };

                
                var response = _service.CreateVehicleMaster(vehicle);

                if (response.statuCode == 1)
                {
                    int Id = response.currentId; 

                    
                    if (DocumentType.Count != StartDate.Count ||
                        StartDate.Count != ExpiryDate.Count ||
                        ExpiryDate.Count != Amount.Count ||
                        Amount.Count != Remarks.Count ||
                        Remarks.Count != Uploads.Count || 
                        Uploads.Count != Docs.Count)
                    {
                        return Json(new { success = false, message = "Mismatch in input data. Ensure all lists have the same number of entries." });
                    }

                   
                    List<VehicleDocumentsEntity> Entries = new List<VehicleDocumentsEntity>();

                    for (int i = 0; i < DocumentType.Count; i++)
                    {
                        // Handle document uploads
                        string uploadFilePath = null;
                        if (Uploads[i] != null && Uploads[i].Length > 0)
                        {
                            var fileName = Path.GetFileName(Uploads[i].FileName);
                            var filePath = Path.Combine("wwwroot/uploads", fileName);

                           
                            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads")))
                            {
                                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads"));
                            }

                           
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                Uploads[i].CopyTo(stream);
                            }

                            uploadFilePath = $"/uploads/{fileName}"; 
                        }

                        var Entry = new VehicleDocumentsEntity
                        {
                            VehicleId = Id, 

                            DocumentType = DocumentType[i],
                            StartDate = StartDate[i],
                            ExpiryDate = ExpiryDate[i],
                            Amount = Amount[i],
                            Remarks = Remarks[i],
                            Uploads = uploadFilePath, 
                            Docs = Docs[i],
                            IsDeleted = false, 
                        };

                        Entries.Add(Entry);
                    }

                   
                    _context.VehicleDocumentsEntitys.AddRange(Entries);
                    _context.SaveChanges();


                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }





        public IActionResult editVehicleMater(int id)
        {

            var tax = _service.GetVehicleMasterById(id);

            if (tax == null)
            {
                return NotFound();
            }

            return View(tax);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult editVehicleMater(VehicleMasterEntity model)
        {
            var response = _service.UpdateVehicleMaster(model);
            if (response.statuCode == 1)
            {
                return Json(new { success = true, message = response.message });
            }
            return Json(new { success = false, message = response.message });
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {

            if (id == 0)
            {
                return BadRequest("Invalid VehicleMaster ID.");
            }


            var response = _service.DeleteVehicleMaster(id);

            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

    }
}
