using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories.Repos
{
    public class TaxMaster : ITaxMaster
    {
        private readonly MyDbContext _context;

        public TaxMaster(MyDbContext context)
        {
            _context = context;
        }

        //-----------------------------Company Setup Master-------------------------------------------

        public IEnumerable<TaxMasterDto> GetTaxMastersWithLedger(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from tax in _context.taxMaster
                        where (tax == null || (tax.IsDeleted == false && tax.IsActive == true))
                        select new TaxMasterDto
                        {
                            TaxId = tax.TaxId,
                            TaxName = tax.TaxName,
                            TaxType = tax.TaxType,
                            Status = tax.Status,
                            IsCommonTax = tax.IsCommonTax,
                            IsTCSTax = tax.IsTCSTax,
                            IsActive = tax.IsActive,
                            IsDeleted = tax.IsDeleted
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.TaxName.Contains(searchTerm) ||
                                          (t.Ledger != null && t.Ledger.Contains(searchTerm)) ||
                                          t.Status.Contains(searchTerm));
            }

            var pagedQuery = query
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize);

            return pagedQuery.ToList();
        }


        public GenericResponse CreateTaxMaster(TaxMasterDto tax)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                // Check if tax with same name and type already exists
                int count = _context.taxMaster.Where(a => a.TaxName == tax.TaxName &&
                                                     a.TaxType == tax.TaxType &&
                                                     a.IsDeleted == false).Count();

                if (count < 1)
                {
                    TaxMasterEntity T = new TaxMasterEntity();

                    T.TaxName = tax.TaxName;
                    T.TaxType = tax.TaxType;
                    T.Status = tax.Status;
                    T.IsCommonTax = tax.IsCommonTax;
                    T.IsTCSTax = tax.IsTCSTax;
                    T.IsDeleted = false;
                    T.IsActive = true;
                    T.CreatedOn = DateTime.Now;
                    T.CreatedBy = 1;

                    _context.taxMaster.Add(T);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Tax created successfully";
                    response.currentId = T.TaxId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "TaxName already exists";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to save Tax: " + ex.Message;
                response.currentId = 0;
            }

            return response;
        }

        public GenericResponse UpdateTaxMaster(TaxMasterDto tax)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                // Check if a different tax with the same name already exists
                var duplicateTax = _context.taxMaster
                    .FirstOrDefault(t => t.TaxName == tax.TaxName &&
                                    t.TaxType == tax.TaxType &&
                                    t.TaxId != tax.TaxId &&
                                    t.IsDeleted == false);

                if (duplicateTax != null)
                {
                    response.statuCode = 0;
                    response.message = "Another tax with this name and type already exists";
                    response.currentId = 0;
                    return response;
                }

                var existingTax = _context.taxMaster
                    .FirstOrDefault(c => c.TaxId == tax.TaxId && c.IsDeleted == false);

                if (existingTax != null)
                {
                    // Update fields
                    existingTax.TaxName = tax.TaxName;
                    existingTax.TaxType = tax.TaxType;
                    existingTax.IsCommonTax = tax.IsCommonTax;
                    existingTax.IsTCSTax = tax.IsTCSTax;
                    existingTax.Status = tax.Status;
                    existingTax.UpdatedOn = DateTime.Now;
                    existingTax.UpdatedBy = 1; // Should use current user ID
                    existingTax.IsActive = true;

                    _context.Update(existingTax);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Tax updated successfully";
                    response.currentId = tax.TaxId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Tax not found";
                    response.currentId = 0;
                }
            }
            catch (DbUpdateException dbEx)
            {
                response.statuCode = 0;
                response.message = "Database error: " + dbEx.Message;
                response.currentId = 0;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to update Tax: " + ex.Message;
                response.currentId = 0;
            }

            return response;
        }


        public GenericResponse DeleteTaxMaster(int id)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                var existingTax = _context.taxMaster
                    .FirstOrDefault(c => c.TaxId == id && c.IsDeleted == false);

                if (existingTax != null)
                {
                    // Soft delete
                    existingTax.IsDeleted = true;
                    existingTax.UpdatedOn = DateTime.Now;
                    existingTax.UpdatedBy = 1; // Should use current user ID

                    _context.Update(existingTax);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Tax deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Tax not found or already deleted";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to delete Tax: " + ex.Message;
                response.currentId = 0;
            }

            return response;
        }
    }
}