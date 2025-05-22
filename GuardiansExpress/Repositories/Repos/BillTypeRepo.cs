using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories.Repos
{
    public class BillTypeRepo : IBillTypeRepo
    {
        private readonly MyDbContext _context;

        public BillTypeRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Bill Master-------------------------------------------

        public IEnumerable<BillEntity> GetBillMaster()
        {
            // Retrieve all bills that are not marked as deleted
            var bills = _context.Bill.Where(b => b.IsDeleted == false).ToList();
            return bills;
        }

        public GenericResponse CreateBillMaster(BillEntity bill)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Check if a bill with the same identifier (e.g., BillNumber) already exists
                var existingBill = _context.Bill.FirstOrDefault(b => b.BillType == bill.BillType); // Replace BillNumber with your unique identifier
                if (existingBill != null)
                {
                    response.statuCode = 0;  // Failure status code
                    response.message = "A bill with the same Bill Number already exists."; // Custom error message
                    response.currentId = 0;  // No ID returned as it's a duplicate
                    return response;  // Exit early if a duplicate is found
                }

                // Set the default value for IsDeleted to false
                bill.IsDeleted = false;

                // Add the bill entity to the context
                _context.Bill.Add(bill);

                // Save changes to the database
                _context.SaveChanges();

                // Return success response
                response.statuCode = 1;
                response.message = "Bill created successfully";
                response.currentId = bill.Id;  // Return the ID of the created bill
            }
            catch (Exception ex)
            {
                // Catch any exceptions and return an error message
                response.statuCode = 0;  // Failure status code
                response.message = "Failed to save Bill: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }

        public BillEntity GetBillById(int id)
        {
            // Retrieve a bill by its ID
            return _context.Bill.Find(id);
        }

        public GenericResponse UpdateBillMaster(BillEntity bill)
            {
            GenericResponse response = new GenericResponse();
            try
            {
                // Check if a bill with the same BillNumber already exists (excluding the current bill being updated)
                var existingBillWithSameNumber = _context.Bill
                    .Where(b => b.BillType == bill.BillType && b.Id != bill.Id)  // Ensures it's not the same bill
                    .FirstOrDefault();

                if (existingBillWithSameNumber != null)
                {
                    response.statuCode = 0;  // Failure status code
                    response.message = "A bill with the same Bill Number already exists."; // Error message
                    response.currentId = 0;  // Set the currentId to 0 as the update failed
                    return response;  // Exit early if a duplicate is found
                }

                // Find the existing bill by its ID
                var existingBill = _context.Bill.Where(b => b.Id == bill.Id).FirstOrDefault();
                if (existingBill != null)
                {

                    existingBill.Id = bill.Id;
                    existingBill.BillType = bill.BillType;
                    existingBill.Status = bill.Status;
                    existingBill.IsDeleted = false;
                    _context.Bill.Update(existingBill);
                    _context.SaveChanges(); // Save the changes

                    response.statuCode = 1;  // Success status code
                    response.message = "Bill updated successfully"; // Success message
                    response.currentId = bill.Id; // Return the ID of the updated bill
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Bill not found"; // Error message if the bill is not found
                    response.currentId = 0; // Set the currentId to 0 if the bill is not found
                }

                return response;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;  // Failure status code
                response.message = "Failed to update Bill: " + ex.Message; // Error message
                response.currentId = 0; // Set the currentId to 0 in case of failure
                return response;
            }
        }


        public GenericResponse DeleteBillMaster(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing bill by its ID
                var existingBill = _context.Bill.Where(b => b.Id == id).FirstOrDefault();
                if (existingBill != null)
                {
                    // Mark the bill as deleted
                    existingBill.IsDeleted = true;
                    _context.Update(existingBill); // Update the bill in the context
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1; // Success status code
                    response.message = "Bill deleted successfully"; // Success message
                    response.currentId = id; // Return the ID of the deleted bill
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Delete Failed"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the bill is not found
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Bill: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }
    }
}
