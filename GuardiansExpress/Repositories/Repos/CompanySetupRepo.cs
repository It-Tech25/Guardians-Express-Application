using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class CompanySetupRepo : ICompanySetupRepo
    {
        private readonly MyDbContext _context;

        public CompanySetupRepo(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Company Setup Master-------------------------------------------

        public IEnumerable<CompanySetupMasterEntity> GetCompanySetupMaster(string searchTerm, int pageNumber, int pageSize)
        {
            // Retrieve all company setup records that are not marked as deleted
            var companySetups = _context.CompanySetups.Where(c => c.IsDeleted == false)
                .ToList();
            return companySetups;
        }

        public GenericResponse CreateCompanySetupMaster(CompanySetupMasterEntity companySetup)
        {
            GenericResponse response = new GenericResponse();
            int count = _context.CompanySetups.Where(a => a.InvoicePrefix == companySetup.InvoicePrefix || a.CNPrefix == companySetup.CNPrefix || a.ONPrefix == companySetup.ONPrefix).Count();
            if (count == 0)
            {
              
                try
                {
                    companySetup.IsDeleted = false; // Set the default value for IsDeleted to false
                    _context.CompanySetups.Add(companySetup); // Add the company setup entity to the context
                    _context.SaveChanges();  // Save changes to the database
                    response.statuCode = 1;  // Success status code
                    response.message = "Company Setup created successfully"; // Success message
                    response.currentId = companySetup.Id; // Return the ID of the created company setup
                }
                catch (Exception ex)
                {
                    response.message = "Failed to save Company Setup: " + ex.Message; // Error message
                    response.currentId = 0;  // Set the currentId to 0 in case of failure
                }
                return response;
            }
            else
            {
                response.message = "InvoicePrefix or CNPrefix Or DNPrefix are doublicated ";
                response.currentId = 0;
                return response;
            }
        }

        public CompanySetupMasterEntity GetCompanySetupMasterById(int id)
        {
            // Retrieve a company setup by its ID
            return _context.CompanySetups.Find(id);
        }

        public GenericResponse UpdateCompanySetupMaster(CompanySetupMasterEntity companySetup)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing company setup by its ID
                var existingCompanySetup = _context.CompanySetups.Where(c => c.Id == companySetup.Id).FirstOrDefault();
                if (existingCompanySetup != null)
                {
                    companySetup.IsDeleted = false;
                    _context.Entry(existingCompanySetup).CurrentValues.SetValues(companySetup);
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1;  // Success status code
                    response.message = "Company Setup updated successfully"; // Success message
                    response.currentId = companySetup.Id; // Return the ID of the updated company setup
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Company Setup not found"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the company setup is not found
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Company Setup: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
                return response;
            }
        }

        public GenericResponse DeleteCompanySetupMaster(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Find the existing company setup by its ID
                var existingCompanySetup = _context.CompanySetups.Where(c => c.Id == id).FirstOrDefault();
                if (existingCompanySetup != null)
                {
                    // Mark the company setup as deleted
                    existingCompanySetup.IsDeleted = true;
                    _context.Update(existingCompanySetup); // Update the company setup in the context
                    _context.SaveChanges(); // Save the changes
                    response.statuCode = 1; // Success status code
                    response.message = "Company Setup deleted successfully"; // Success message
                    response.currentId = id; // Return the ID of the deleted company setup
                }
                else
                {
                    response.statuCode = 0; // Failure status code
                    response.message = "Delete Failed"; // Error message
                    response.currentId = 0; // Set the currentId to 0 if the company setup is not found
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Company Setup: " + ex.Message; // Error message
                response.currentId = 0;  // Set the currentId to 0 in case of failure
            }
            return response;
        }
    }
}
