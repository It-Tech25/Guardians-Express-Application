using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IAddressBookMasterService 
    {
        IEnumerable<AddressBookMasterDTO> GetAddressBooks();
        AddressBookMasterDTO GetAddressBookByClientId(int clientId);
        GenericResponse CreateContact(AddressBookMasterDTO addressBookDto);
        GenericResponse UpdateContact(AddressBookMasterDTO addressBookDto);
        GenericResponse DeleteContact(int clientId);
    }
}
