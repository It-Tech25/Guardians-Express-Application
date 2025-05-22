using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IAddressBookMaster
    {
        IEnumerable<AddressBookMasterDTO> GetAddressBooks();
        AddressBookMasterDTO GetAddressBookByClientId(int clientId);
        GenericResponse CreateAddressBook(AddressBookMasterDTO addressBookDto);
        GenericResponse UpdateAddressBook(AddressBookMasterDTO addressBookDto);
        GenericResponse DeleteAddressBook(int clientId);
    }
}
