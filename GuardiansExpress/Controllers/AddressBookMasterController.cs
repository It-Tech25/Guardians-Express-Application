using GuardiansExpress.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;


namespace GuardiansExpress.Controllers
{
    public class AddressBookMasterController : Controller
    {
        private readonly IAddressBookMasterService _addressBookService;
        private readonly MyDbContext _context;
        // Constructor injection for AddressBookService
        public AddressBookMasterController(IAddressBookMasterService addressBookService,MyDbContext context)
        {
            _addressBookService = addressBookService;
            _context = context;
        }

        //----------------------------- Get All Contacts -------------------------------------------
        public IActionResult AddressBookIndex()
        {
            LoginResponse lr = new LoginResponse();
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
           var state = _context.stateEntities.Select(a => a.StateName). ToList();
            ViewBag.state = state;
            // Pass user details to the view
            ViewBag.UserName = loggedInUser.userName;
            ViewBag.UserEmail = loggedInUser.Emailid;
            ViewBag.UserRole = loggedInUser.Role;
            //ViewBag.UserProfileImage = loggedInUser.ProfileImageUrl; // URL to the user's profile image
            var res = _addressBookService.GetAddressBooks().Select(entity => new AddressBookMasterDTO
            {
                ClientId = entity.ClientId,
                ClientName = entity.ClientName,
                BillParty = entity.BillParty,
                ContactPersonName = entity.ContactPersonName,
                Mobile = entity.Mobile,
                Email = entity.Email,
                Address = entity.Address,
                City = entity.City,
                State = entity.State,
                Country = entity.Country,
                Pincode = entity.Pincode,
                GSTNo = entity.GSTNo
            }).ToList();
            return View(res);
        }


        //----------------------------- Add Contact -------------------------------------------
        [HttpPost]
        public IActionResult AddContact(AddressBookMasterDTO contact)
        {
            //var entity = new AddressBookMasterEntity
            //{
            //    ClientId = contact.ClientId,
            //    ClientName = contact.ClientName,
            //    BillParty = contact.BillParty,
            //    ContactPersonName = contact.ContactPersonName,
            //    Mobile = contact.Mobile,
            //    Email = contact.Email,
            //    Address = contact.Address,
            //    City = contact.City,
            //    State = contact.State,
            //    Country = contact.Country,
            //    Pincode = contact.Pincode,
            //    GSTNo = contact.GSTNo
            //};

            // Call service to create a new contact
            var response = _addressBookService.CreateContact(contact);
            if (response.statuCode == 1)
            {
                return Json(new { success = true }); 
            }
            else
            {
                return Json(new { success = false, message = response.message });
            }
        }


        //----------------------------- Edit Contact (GET) -------------------------------------------
        public IActionResult EditContact(int id)
        {
            // Retrieve the contact by its ID for editing
            var contact = _addressBookService.GetAddressBookByClientId(id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        //----------------------------- Edit Contact (POST) -------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateContact(AddressBookMasterDTO contact)
        {

            //var entity = new AddressBookMasterEntity
            //{
            //    ClientId = contact.ClientId,
            //    ClientName = contact.ClientName,
            //    BillParty = contact.BillParty,
            //    ContactPersonName = contact.ContactPersonName,
            //    Mobile = contact.Mobile,
            //    Email = contact.Email,
            //    Address = contact.Address,
            //    City = contact.City,
            //    State = contact.State,
            //    Country = contact.Country,
            //    Pincode = contact.Pincode,
            //    GSTNo = contact.GSTNo
            //};
            var response = _addressBookService.UpdateContact(contact);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = response.message });
        }

        //----------------------------- Delete Contact -------------------------------------------
        [HttpPost]
        
        public IActionResult Delete(int id)
        {
            // Check for invalid ID
            if (id == 0)
            {
                return BadRequest("Invalid Contact ID.");
            }

            // Call service to delete the contact (soft delete by setting IsDeleted = true)
            var response = _addressBookService.DeleteContact(id);

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

