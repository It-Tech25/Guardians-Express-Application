using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using System.Collections.Generic;

namespace GuardiansExpress.Services
{
    public class BillTypeService : IBillTypeService
    {
        private readonly IBillTypeRepo _billTypeRepo;

        // Constructor
        public BillTypeService(IBillTypeRepo billTypeRepo)
        {
            _billTypeRepo = billTypeRepo;
        }

        // Method to get all BillTypes (for a master list or other purposes)
        public IEnumerable<BillEntity> GetBillMaster()
        {
            return _billTypeRepo.GetBillMaster();
        }

        // Method to create a new BillType record
        public GenericResponse CreateBillMaster(BillEntity billType)
        {
            return _billTypeRepo.CreateBillMaster(billType);
        }

        // Method to get a BillType by ID
        public BillEntity GetBillById(int id)
        {
            return _billTypeRepo.GetBillById(id);
        }

        // Method to update an existing BillType record
        public GenericResponse UpdateBillMaster(BillEntity bill)
        {
            return _billTypeRepo.UpdateBillMaster(bill);
        }

        // Method to delete a BillType record by ID
        public GenericResponse DeleteBillMaster(int id)
        {
            return _billTypeRepo.DeleteBillMaster(id);
        }
    }
}
