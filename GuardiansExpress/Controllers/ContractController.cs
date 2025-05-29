using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace GuardiansExpress.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractService _service;
        private readonly IBranchMasterService _branchService;
        private readonly IInvoiceTypeService _invoiceService;
        private readonly MyDbContext _context;

        public ContractController(IContractService service, IBranchMasterService branchService,
                                 IInvoiceTypeService invoiceService, MyDbContext context)
        {
            _service = service;
            _branchService = branchService;
            _invoiceService = invoiceService;
            _context = context;
        }

        public IActionResult ContractIndex()
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
            var contracts = _service.GetContracts();
            var totalRecords = contracts.Count();
            //  var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);



            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            ViewBag.Branches = branches;

            var invoices = _invoiceService.GetInvoiceTypes().ToList();
            ViewBag.Invoices = invoices;

            return View(contracts);
        }

        public IActionResult Details(int id)
        {
            var contract = _service.GetContractById(id);
            if (contract == null)
            {
                return NotFound();
            }

            // Get related contract items
            var contractItems = _context.contractEntities
                .Where(item => item.ContractId == id)
                .ToList();

            ViewBag.ContractItems = contractItems;

            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            ViewBag.Branches = branches;

            var invoices = _invoiceService.GetInvoiceTypes().ToList();
            ViewBag.Invoices = invoices;

            return View(contract);
        }

        public IActionResult Create()
        {
            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            ViewBag.Branches = branches;

            var invoices = _invoiceService.GetInvoiceTypes().ToList();
            ViewBag.Invoices = invoices;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateContract(
            ContractModel model,
            List<string> Item,
            List<string> BillingItem,
            List<string> Description,
            List<string> SalesAccount,
            List<decimal> Rate,
            List<string> Qty,
            List<decimal> Amount,
            List<decimal?> ItemContractPercentage,
            List<DateTime> StartDate,
            List<DateTime> EndDate,
            List<decimal> TotalAmount)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            try
            {
                // Create the main Contract entity
                var contract = new ContractEntity
                {
                    BranchMasterId = model.BranchMasterId,
                    InvoiceId = model.InvoiceId,
                    DisableContract = model.DisableContract,
                    AutoInvoice = model.AutoInvoice,
                    TempClose = model.TempClose,
                    ClientName = model.ClientName,
                    ReferenceName = model.ReferenceName,
                    InvoiceType = model.InvoiceType,
                    ContractType = model.ContractType,
                    ContractEndDate = model.ContractEndDate,
                    EndRental = model.EndRental,
                    EmailReminder = model.EmailReminder,
                    SMSReminder = model.SMSReminder,
                    WhatsAppReminder = model.WhatsAppReminder,
                    IsActive = true,
                    IsDeleted = false
                };

                // Call the service layer to add the contract
                var response = _service.CreateContract(contract);

                if (response.statuCode == 1)
                {
                    int contractId = response.currentId; // Get the ID of the newly inserted record

                    // Validate that all input lists have the same length
                    int itemCount = Item.Count;
                    if (itemCount == null)
                    {
                        return Json(new { success = false, message = "Mismatch in input data. Ensure all lists have the same number of entries." });
                    }

                    // Save Contract Items
                    List<ContractItemEntity> contractItems = new List<ContractItemEntity>();

                    for (int i = 0; i < itemCount; i++)
                    {
                        var contractItem = new ContractItemEntity
                        {
                            ContractId = contractId,
                            MaterialDescription = Item[i],
                            FromPlace = BillingItem[i],
                            ToPlace = Description[i],
                            VehicleType = SalesAccount[i],
                            FreightRate = Rate[i],
                            VehicleSize = Qty[i],
                            StartDate = StartDate[i],
                            EndDate = EndDate[i],
                            IsActive = true,
                            IsDeleted = false
                        };

                        contractItems.Add(contractItem);
                    }

                    // Add contract items to the database
                    _context.contractItemEntities.AddRange(contractItems);
                    _context.SaveChanges();

                    return RedirectToAction("ContractIndex");
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

        public IActionResult GetContractById(int id)
        {
            var contract = _service.GetContractById(id);
            if (contract == null)
            {
                return NotFound();
            }

            // Get related contract items
            var contractItems = _context.contractEntities
                .Where(item => item.ContractId == id)
                .Select(item => new
                {
                    item.ContractId,
                    item.ClientName
                    // Add other necessary fields here...
                })
                .ToList();

            // Get related data
            var branches = _branchService.BranchMaster(string.Empty, 1, 100).ToList();
            var invoices = _invoiceService.GetInvoiceTypes().ToList();

            // Return JSON object including contract details, items, and additional data
            return Json(new
            {
                contractId = contract.ContractId,
                branchMasterId = contract.BranchMasterId,
                invoiceId = contract.InvoiceId,
                disableContract = contract.DisableContract,
                autoInvoice = contract.AutoInvoice,
                tempClose = contract.TempClose,
                clientName = contract.ClientName,
                referenceName = contract.ReferenceName ?? "",
                invoiceType = contract.InvoiceType ?? "",
                contractType = contract.ContractType,
                contractStartDate = contract.ContractEndDate?.ToString("yyyy-MM-dd") ?? "",
                contractEndDate = contract.ContractEndDate?.ToString("yyyy-MM-dd") ?? "",
                endRental = contract.EndRental ?? false,
                emailReminder = contract.EmailReminder ?? false,
                smsReminder = contract.SMSReminder ?? false,
                whatsAppReminder = contract.WhatsAppReminder ?? false,
                isActive = contract.IsActive ?? false,
                items = contractItems,
                branches = branches,
                invoices = invoices
            });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContractEdit(
            ContractEntity model,
            List<int> ItemId,
            List<string> Item,
            List<string> BillingItem,
            List<string> Description,
            List<string> SalesAccount,
            List<decimal> Rate,
            List<string> Qty,
            List<decimal> Amount,
            List<decimal?> ItemContractPercentage,
            List<DateTime> StartDate,
            List<DateTime> EndDate,
            List<decimal> TotalAmount)
        {
            try
            {
                // Update contract

                var response = _service.UpdateContract(model);

                if (response.statuCode == 1)
                {
                    // Remove existing contract items
                    var existingItems = _context.contractEntities
                        .Where(item => item.ContractId == model.ContractId)
                        .ToList();

                    _context.contractEntities.RemoveRange(existingItems);
                    _context.SaveChanges();

                    // Add updated contract items
                    List<ContractItemEntity> contractItems = new List<ContractItemEntity>();

                    for (int i = 0; i < Item.Count; i++)
                    {
                        var contractItem = new ContractItemEntity
                        {
                            ContractId = model.ContractId,
                            MaterialDescription = Item[i],
                            FromPlace = BillingItem[i],
                            ToPlace = Description[i],
                            VehicleType = SalesAccount[i],
                            FreightRate = Rate[i],
                            VehicleSize = Qty[i],
                            StartDate = StartDate[i],
                            EndDate = EndDate[i],
                            IsActive = true,
                            IsDeleted = false
                        };

                        contractItems.Add(contractItem);
                    }

                    _context.contractItemEntities.AddRange(contractItems);
                    _context.SaveChanges();

                    return RedirectToAction("ContractIndex");
                }

                return Json(new { success = false, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public IActionResult DeleteContract(int id)
        {
            if (id == 0)
            {
                return Json(new { success = false, message = "Invalid Contract ID." });
            }

            try
            {
                var response = _service.DeleteContract(id);

                if (response.statuCode == 1)
                {
                    return Json(new { success = true });
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
    }
}
