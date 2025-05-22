using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Services
{
    public class AddressBookMasterService : IAddressBookMasterService
    {
        private readonly IAddressBookMaster _addressBookMasterRepo;

        public AddressBookMasterService(IAddressBookMaster addressBookMasterRepo)
        {
            _addressBookMasterRepo = addressBookMasterRepo;
        }

        public IEnumerable<AddressBookMasterDTO> GetAddressBooks()
        {
            return _addressBookMasterRepo.GetAddressBooks();
        }

        public AddressBookMasterDTO GetAddressBookByClientId(int clientId)
        {
            return _addressBookMasterRepo.GetAddressBookByClientId(clientId);
        }

        public GenericResponse CreateContact(AddressBookMasterDTO addressBookDto)
        {
            return _addressBookMasterRepo.CreateAddressBook(addressBookDto);
        }

        public GenericResponse UpdateContact(AddressBookMasterDTO addressBookDto)
        {
            return _addressBookMasterRepo.UpdateAddressBook(addressBookDto);
        }

        public GenericResponse DeleteContact(int clientId)
        {
            return _addressBookMasterRepo.DeleteAddressBook(clientId);
        }
    }
}
