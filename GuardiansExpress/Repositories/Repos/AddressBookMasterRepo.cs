using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class AddressBookMasterRepo : IAddressBookMaster
    {
        private readonly MyDbContext _context;

        public AddressBookMasterRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Address Book-------------------------------------------

        // Get all address book records (excluding deleted ones)
        public IEnumerable<AddressBookMasterDTO> GetAddressBooks()
        {
            var addressBooks = _context.AddressBookMaster
                .Where(a => a.IsDeleted == false && a.IsActive==true)
                .Select(a => new AddressBookMasterDTO
                {
                    ClientId = a.ClientId,
                    ClientName = a.ClientName,
                    BillParty = a.BillParty,
                    ContactPersonName = a.ContactPersonName,
                    Mobile = a.Mobile,
                    Email = a.Email,
                    Address = a.Address,
                    City = a.City,
                    State = a.State,
                    Country = a.Country,
                    Pincode = a.Pincode,
                    GSTNo = a.GSTNo,
                    IsDeleted = a.IsDeleted,
                    IsActive = a.IsActive,
                    CreatedOn = a.CreatedOn,
                    CreatedBy = a.CreatedBy,
                    UpdatedOn = a.UpdatedOn,
                    UpdatedBy = a.UpdatedBy
                })
                .ToList();

            return addressBooks;
        }

        // Get address book record by client ID
        public AddressBookMasterDTO GetAddressBookByClientId(int clientId)
        {
            var addressBook = _context.AddressBookMaster
                .Where(a => a.ClientId == clientId && a.IsDeleted == false)
                .Select(a => new AddressBookMasterDTO
                {
                    ClientId = a.ClientId,
                    ClientName = a.ClientName,
                    BillParty = a.BillParty,
                    ContactPersonName = a.ContactPersonName,
                    Mobile = a.Mobile,
                    Email = a.Email,
                    Address = a.Address,
                    City = a.City,
                    State = a.State,
                    Country = a.Country,
                    Pincode = a.Pincode,
                    GSTNo = a.GSTNo,
                    IsDeleted = a.IsDeleted,
                    IsActive = a.IsActive,
                    CreatedOn = a.CreatedOn,
                    CreatedBy = a.CreatedBy,
                    UpdatedOn = a.UpdatedOn,
                    UpdatedBy = a.UpdatedBy
                })
                .FirstOrDefault();

            return addressBook;
        }

        // Create a new address book record
        public GenericResponse CreateAddressBook(AddressBookMasterDTO addressBookDto)
        {int count=_context.AddressBookMaster.Where(a=>a.Address==addressBookDto.Address && a.IsDeleted==false).Count();
            GenericResponse response = new GenericResponse();
            if (count < 1)
            {
                try
                {
                    // Check if ClientId already exists
                    var existingClient = _context.AddressBookMaster
                        .Any(a => a.ClientId == addressBookDto.ClientId);
                    if (existingClient)
                    {
                        response.statuCode = 0;
                        response.message = "Client already exists.";
                        response.currentId = 0;
                        return response;
                    }

                    var addressBook = new AddressBookMasterEntity
                    {
                        ClientId = addressBookDto.ClientId,
                        ClientName = addressBookDto.ClientName,
                        BillParty = addressBookDto.BillParty,
                        ContactPersonName = addressBookDto.ContactPersonName,
                        Mobile = addressBookDto.Mobile,
                        Email = addressBookDto.Email,
                        Address = addressBookDto.Address,
                        City = addressBookDto.City,
                        State = addressBookDto.State,
                        Country = addressBookDto.Country,
                        Pincode = addressBookDto.Pincode,
                        GSTNo = addressBookDto.GSTNo,
                        IsDeleted = false,
                        IsActive = true,
                        CreatedOn = addressBookDto.CreatedOn ?? DateTime.Now,
                        CreatedBy = addressBookDto.CreatedBy,
                        UpdatedOn = addressBookDto.UpdatedOn,
                        UpdatedBy = addressBookDto.UpdatedBy
                    };

                    _context.AddressBookMaster.Add(addressBook);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Address Book entry created successfully";
                    response.currentId = addressBook.ClientId;
                }
                catch (Exception ex)
                {
                    response.message = "Failed to create Address Book entry: " + ex.Message;
                    response.currentId = 0;
                }
            }
            else
            {
                response.message = "Address Alredy Exist ";
                response.currentId = 0;
            }

            return response;
        }

        // Update an existing address book record
        public GenericResponse UpdateAddressBook(AddressBookMasterDTO addressBookDto)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingAddressBook = _context.AddressBookMaster
                    .FirstOrDefault(a => a.ClientId == addressBookDto.ClientId);

                if (existingAddressBook != null)
                {
                    _context.Entry(existingAddressBook).CurrentValues.SetValues(addressBookDto);
                    existingAddressBook.UpdatedOn = DateTime.Now;
                    existingAddressBook.IsDeleted = false;
                    existingAddressBook.IsActive = true;

                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Address Book entry updated successfully";
                    response.currentId = addressBookDto.ClientId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Address Book entry not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Address Book entry: " + ex.Message;
                response.currentId = 0;
            }

            return response;
        }

        // Soft delete an address book record
        public GenericResponse DeleteAddressBook(int clientId)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingAddressBook = _context.AddressBookMaster
                    .FirstOrDefault(a => a.ClientId == clientId);

                if (existingAddressBook != null)
                {
                    existingAddressBook.IsDeleted = true;
                    existingAddressBook.IsActive = false;
                    existingAddressBook.UpdatedOn = DateTime.Now;
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Address Book entry deleted successfully";
                    response.currentId = clientId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete failed: Address Book entry not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Address Book entry: " + ex.Message;
                response.currentId = 0;
            }

            return response;    
        }
    }
}
