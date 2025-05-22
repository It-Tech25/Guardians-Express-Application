using Azure.Core;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GuardiansExpress.Controllers
{
    public class CompanyConfigurationController : Controller
    {
        private readonly ICompanyConfigurationService _companyConfigService;
        private readonly MyDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        // Constructor injection for CompanyConfigurationService
        public CompanyConfigurationController(ICompanyConfigurationService companyConfigService, MyDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _companyConfigService = companyConfigService;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        //----------------------------- Get All Company Configurations -------------------------------------------
        public IActionResult CompanyConfigurationIndex(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            LoginResponse lr = new LoginResponse();
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
            var query = _context.CompanyConfigurations
                        .Where(a => a.IsDeleted == false &&
                                   (string.IsNullOrEmpty(searchTerm) || a.Name.Contains(searchTerm)));

            // Get total count after filtering
            int totalRecords = query.Count();

            // Apply pagination
            var configurations = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CompanyConfigurationViewModel
                {
                     CompanyId = x.CompanyId,
                                  Name = x.Name,
                                  Email = x.Email,
                                  InfoEmails = x.InfoEmails,
                                  Phone = x.Phone,
                                  TelNo = x.TelNo,
                                  Fax = x.Fax,
                                  Website = x.Website,
                                  PANNo = x.PANNo,
                                  GSTIN = x.GSTIN,
                                  CINNumber = x.CINNumber,
                                  HSCSSN = x.HSCSSN,


                                  CompanyLogoPath = x.CompanyLogoPath,
                                  LetterHeadImagePath = x.LetterHeadImagePath,
                                  EnableSMS = x.EnableSMS,
                                  EnableEmail = x.EnableEmail,
                                  IsDefault = x.IsDefault,
                                  ChangeTaxOnInvoice = x.ChangeTaxOnInvoice,
                                  ShowItemImageInLead = x.ShowItemImageInLead,
                                  ShowItemImageInPO = x.ShowItemImageInPO,
                                  EnableKYCVerification = x.EnableKYCVerification,
                                  CreatedOn = x.CreatedOn,
                                  UpdatedOn = x.UpdatedOn,
                                  CreatedBy = x.CreatedBy,
                                  UpdatedBy = x.UpdatedBy,
                                  IsDeleted = x.IsDeleted,
                                  AddressLine1 = x.AddressLine1,
                                  AddressLine2 = x.AddressLine2,
                                  City = x.City,
                                  State = x.State,
                                  Country = x.Country,
                                  Pincode = x.Pincode,
                                 
                                  RoundOffInvoice = x.RoundOffInvoice,
                                  RequiredTransportDetails = x.RequiredTransportDetails,
                                  ServiceIndustry = x.ServiceIndustry,
                                  DuplicateItemInBill = x.DuplicateItemInBill,
                                  ImportGoods = x.ImportGoods,
                                  BarcodeBilling = x.BarcodeBilling,
                                  BarcodeOnSrno = x.BarcodeOnSrno,
                                  ExportTCSApplicable = x.ExportTCSApplicable,
                                  ProvisionForBilling = x.ProvisionForBilling,
                                  ManualWtInBilling = x.ManualWtInBilling,
                                  ApprovedVoucherInLedger = x.ApprovedVoucherInLedger,
                                  ManualRate = x.ManualRate,
                                  DispatchWiseGR = x.DispatchWiseGR,
                                  TransShipment = x.TransShipment,
                                  ClubVoucherEntryTotal = x.ClubVoucherEntryTotal,
                                  ShowGroupInVoucherEntry = x.ShowGroupInVoucherEntry,
                                  DirectDebitDriverTransporter = x.DirectDebitDriverTransporter,
                                  ResetGRNoFinancialYearWise = x.ResetGRNoFinancialYearWise,
                                  UpcomingEMI = x.UpcomingEMI,
                                  LedgerWiseDetail = x.LedgerWiseDetail,
                                  LockEntryTill = x.LockEntryTill,
                                  TCSAmount = x.TCSAmount,
                                  InvoiceNoMinLength = x.InvoiceNoMinLength,
                                  ContractDueDays = x.ContractDueDays,
                                  EwayBillAmount = x.EwayBillAmount,
                                  ORCPercentage = x.ORCPercentage,
                                  AutoInvoiceTime = x.AutoInvoiceTime,
                                  PasswordResetDays = x.PasswordResetDays,
                                  URLExpireTime = x.URLExpireTime,
                                  Status = x.Status,
                                  WhiteListIP = x.WhiteListIP,
                                  InvoiceTemplate = x.InvoiceTemplate,
                                  DebitNoteTemplate = x.DebitNoteTemplate,
                                  LeadTemplate = x.LeadTemplate,
                                  GRTemplate = x.GRTemplate,
                                  GRReceiveTemplate = x.GRReceiveTemplate,
                                  BillTemplate = x.BillTemplate,
                                  BillDetails1Template = x.BillDetails1Template,
                                  BillDetails2Template = x.BillDetails2Template,
                                  HireTemplate = x.HireTemplate,
                                  HireTransporterTemplate = x.HireTransporterTemplate,
                                  SaleOrderTemplate = x.SaleOrderTemplate,
                                  DefaultTemplate = x.DefaultTemplate,
                                  LeadPDFTitle = x.LeadPDFTitle,
                                  SOTopHeight = x.SOTopHeight,
                                  SOHeaderHeight = x.SOHeaderHeight,
                                  SOFooterHeight = x.SOFooterHeight,
                                  LeadTopHeight = x.LeadTopHeight,
                                  LeadHeaderHeight = x.LeadHeaderHeight,
                                  LeadFooterHeight = x.LeadFooterHeight,
                                  InvTopHeight = x.InvTopHeight,
                                  InvHeaderHeight = x.InvHeaderHeight,
                                  InvFooterHeight = x.InvFooterHeight,
                                  GRTopHeight = x.GRTopHeight,
                                  GRHeaderHeight = x.GRHeaderHeight,
                                  GRFooterHeight = x.GRFooterHeight,
                                  BillTopHeight = x.BillTopHeight,
                                  BillHeaderHeight = x.BillHeaderHeight,
                                  BillFooterHeight = x.BillFooterHeight,
                                  Bill1TopHeight = x.Bill1TopHeight,
                                  Bill1HeaderHeight = x.Bill1HeaderHeight,
                                  Bill1FooterHeight = x.Bill1FooterHeight,
                                  Bill2TopHeight = x.Bill2TopHeight,
                                  Bill2HeaderHeight = x.Bill2HeaderHeight,
                                  Bill2FooterHeight = x.Bill2FooterHeight,
                                  HireTopHeight = x.HireTopHeight,
                                  HireHeaderHeight = x.HireHeaderHeight,
                                  HireFooterHeight = x.HireFooterHeight,
                                  HireTransporterTopHeight = x.HireTransporterTopHeight,
                                  HireTransporterHeaderHeight = x.HireTransporterHeaderHeight,
                                  HireTransporterFooterHeight = x.HireTransporterFooterHeight,
                                  CalDashEventCount = x.CalDashEventCount,
                                  EInvoiceUsername = x.EInvoiceUsername,
                                  EInvoicePassword = x.EInvoicePassword
                })
                .ToList();

            // Set view data
            ViewData["PageSize"] = pageSize;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(configurations);
        }

        //----------------------------- Add Company Configuration -------------------------------------------
        [HttpPost]
        public IActionResult CompanyConfigurationAdd(CompanyConfigurationViewModel model, IFormFile companyLogo, IFormFile letterHeadImage)
        {
            bool isCompanyLogoUploaded = false;
            bool isLetterHeadImageUploaded = false;
            string companyLogoUrl = "dummy.png";   
            string letterHeadImageUrl = "dummy.png";  

             if (companyLogo != null)
            {
                var companyLogoFileName = Path.GetFileName(companyLogo.FileName);
                if (companyLogoFileName != null)
                {
                    var contentType = companyLogo.ContentType;
                    string filename = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                    filename = Regex.Replace(filename, "[A-Za-z ]", "");
                    filename = filename + RandomGenerator.RandomString(4, false);
                    string extension = Path.GetExtension(companyLogoFileName);
                    filename += extension;

                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, filename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        companyLogo.CopyTo(fileStream);
                    }
                    companyLogoUrl = filename;   
                    isCompanyLogoUploaded = true;
                }
            }

             if (letterHeadImage != null)
            {
                var letterHeadImageFileName = Path.GetFileName(letterHeadImage.FileName);
                if (letterHeadImageFileName != null)
                {
                    var contentType = letterHeadImage.ContentType;
                    string filename = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                    filename = Regex.Replace(filename, "[A-Za-z ]", "");
                    filename = filename + RandomGenerator.RandomString(4, false);
                    string extension = Path.GetExtension(letterHeadImageFileName);
                    filename += extension;

                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, filename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        letterHeadImage.CopyTo(fileStream);
                    }
                    letterHeadImageUrl = filename;   
                    isLetterHeadImageUploaded = true;
                }
            }
            model.CompanyLogoPath = companyLogoUrl;
            model.LetterHeadImagePath = letterHeadImageUrl;


            var response = _companyConfigService.CreateCompanyConfiguration(model);
            if (response.statuCode == 1)
            {
                model.CompanyLogoPath= companyLogoUrl;
                model.LetterHeadImagePath = letterHeadImageUrl;

                 UploadFiles(companyLogo, letterHeadImage).Wait();

                return Json(new { success = true, message = response.message });

            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }


        //----------------------------- Edit Company Configuration (GET) -------------------------------------------
        public IActionResult Edit(int id)
        {
            // Retrieve the Company Configuration by its ID for editing
            var configuration = _companyConfigService.GetCompanyConfigurationById(id);

            if (configuration == null)
            {
                return NotFound();
            }

            return View(configuration);
        }

        //----------------------------- Edit Company Configuration (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompanyConfigurationUpdate(CompanyConfigurationViewModel model)
        {
            var response = _companyConfigService.UpdateCompanyConfiguration(model);

            if (response.statuCode == 1)
            {

                return RedirectToAction("CompanyConfigurationIndex");

            }

            // If the response status code is not 1, return the error message as JSON
            return Json(new { success = false, message = response.message });
        }


        //----------------------------- Delete Company Configuration -------------------------------------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return BadRequest("Invalid Company Configuration ID.");
            }

            // Call service to delete the company configuration
            var response = _companyConfigService.DeleteCompanyConfiguration(id);

            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }

        //----------------------------- Get Company Configuration Details -------------------------------------------
        public IActionResult Details(int id)
        {
            var configuration = _companyConfigService.GetCompanyConfigurationById(id);
            if (configuration == null)
            {
                return NotFound();
            }
            return Json(new { success = true }); 
        }

        public async Task<IActionResult> UploadFiles(IFormFile companyLogo, IFormFile letterHeadImage)
        {
            if (companyLogo != null && companyLogo.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", companyLogo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await companyLogo.CopyToAsync(stream);
                }

                var companyLogoPath = "/uploads/" + companyLogo.FileName;
                await _companyConfigService.SaveCompany(companyLogoPath, null);
            }

            if (letterHeadImage != null && letterHeadImage.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", letterHeadImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await letterHeadImage.CopyToAsync(stream);
                }

                var letterHeadImagePath = "/uploads/" + letterHeadImage.FileName;
                await _companyConfigService.SaveCompany(null, letterHeadImagePath);
            }

            return Json(new { success = true }); 
        }   
    }
}