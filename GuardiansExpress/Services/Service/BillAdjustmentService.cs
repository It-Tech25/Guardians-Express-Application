using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class BillAdjustmentService : IBillAdjustmentService
    {
        private readonly IBillAdjustmentRepo _Repo;

        public BillAdjustmentService(IBillAdjustmentRepo Repo)
        {
            _Repo = Repo;
        }
        public IEnumerable<BillAdjustmentDTO> GetBillAdjustment(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {

                return _Repo.GetBillAdjustment(searchTerm, pageNumber, pageSize);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving Bill Adjustment.", ex);
            }
        }
        public BillAdjustmentEntity GetBillAdjustmentById(int id)
        {
            try
            {

                return _Repo.GetBillAdjustmentById(id);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving the BillAdjustment by ID.", ex);
            }
        }
        public GenericResponse CreateBillAdjustment(BillAdjustmentEntity Bill)
        {
            try
            {

                return _Repo.CreateBillAdjustment(Bill);
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while creating the BillAdjustment.",
                    currentId = 0
                };
            }
        }
        public GenericResponse UpdateBillAdjustment(BillAdjustmentEntity Bill)
        {
            try
            {

                return _Repo.UpdateBillAdjustment(Bill);
            }
            catch (Exception ex)
            {

                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while updating the BillAdjustment.",
                    currentId = 0
                };
            }
        }
        public GenericResponse DeleteBillAdjustment(int id)
        {
            try
            {

                return _Repo.DeleteBillAdjustment(id);
            }
            catch (Exception ex)
            {

                return new GenericResponse
                {
                    statuCode = 0,
                    message = "An error occurred while deleting the Vehicle Master.",
                    currentId = 0
                };
            }

        }
    }
}
