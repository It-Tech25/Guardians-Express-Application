using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class InvoiceReturnRepo : IInvoiceReturnRepo
    {
        private readonly MyDbContext _context;

        public InvoiceReturnRepo(MyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<InvoiceReturnModel> GetInvoiceReturns()
        {
            try
            {
                var query = from ir in _context.invoiceReturns
                            join b in _context.branch
                            on ir.BranchID equals b.id
                            where ir.IsDeleted==false // Ensure only non-deleted records are selected
                            select new InvoiceReturnModel
                            {
                                InvoiceReturnID = ir.InvoiceReturnID,
                                BranchID = ir.BranchID,
                               BranchName = b.BranchName,  // Dropdown Value
                                AddressID = ir.AddressID,
                                //Address = address.Address,  // Dropdown Value
                                InvoiceReturnDate = ir.InvoiceReturnDate,
                                AccHead = ir.AccHead,
                                InvoiceNo = ir.InvoiceNo,
                                SalesType = ir.SalesType,
                                InvoiceDate = ir.InvoiceDate,
                                InvoiceAmount = ir.InvoiceAmount,
                                AccGSTIN = ir.AccGSTIN,
                                CostCenter = ir.CostCenter,
                                NoGST = ir.NoGST,
                                DiscountOnMRP = ir.DiscountOnMRP,
                                Notes = ir.Notes,
                                GrossAmount = ir.GrossAmount,
                                Discount = ir.Discount,
                                Tax = ir.Tax,
                                RoundOff = ir.RoundOff,
                                NetAmount = ir.NetAmount,
                                IsActive = ir.IsActive,
                                IsDeleted = ir.IsDeleted
                            };

                // Execute the query and convert the results into a list
                var result = query.ToList();

                // Log or check the number of records fetched
                if (result.Count == 0)
                {
                    Console.WriteLine("No data found.");
                }
                else
                {
                    Console.WriteLine($"{result.Count} records found.");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception message
                Console.WriteLine($"Error: {ex.Message}");
                return new List<InvoiceReturnModel>(); // Return an empty list in case of an error
            }
        }



        public IEnumerable<InvoiceReturnModel> GetInvoiceReturns(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from ir in _context.invoiceReturns
                        join branch in _context.branch on ir.BranchID equals branch.id
                        join address in _context.AddressBookMaster on ir.AddressID equals address.ClientId
                        where ir.IsDeleted==false
                        select new InvoiceReturnModel
                        {
                            InvoiceReturnID = ir.InvoiceReturnID,
                            BranchID = ir.BranchID,
                            BranchName = branch.BranchName,  // Dropdown Value
                            AddressID = ir.AddressID,
                            Address = address.Address,  // Dropdown Value
                            InvoiceReturnDate = ir.InvoiceReturnDate,
                            AccHead = ir.AccHead,
                            InvoiceNo = ir.InvoiceNo,
                            SalesType = ir.SalesType,
                            InvoiceDate = ir.InvoiceDate,
                            InvoiceAmount = ir.InvoiceAmount,
                            AccGSTIN = ir.AccGSTIN,
                            CostCenter = ir.CostCenter,
                            NoGST = ir.NoGST,
                            DiscountOnMRP = ir.DiscountOnMRP,
                            Notes = ir.Notes,
                            GrossAmount = ir.GrossAmount,
                            Discount = ir.Discount,
                            Tax = ir.Tax,
                            RoundOff = ir.RoundOff,
                            NetAmount = ir.NetAmount,
                            IsActive = ir.IsActive,
                            IsDeleted = ir.IsDeleted
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ir => ir.InvoiceNo.Contains(searchTerm) || ir.BranchName.Contains(searchTerm));
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }


        public InvoiceReturnEntity GetInvoiceReturnById(int id)
        {
            return _context.invoiceReturns.Find(id);
        }

        public GenericResponse CreateInvoiceReturn(InvoiceReturnEntity invoiceReturn)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                invoiceReturn.IsDeleted = false;
                _context.invoiceReturns.Add(invoiceReturn);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Invoice Return created successfully";
                response.currentId = invoiceReturn.InvoiceReturnID;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Invoice Return: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse UpdateInvoiceReturn(InvoiceReturnEntity invoiceReturn)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.invoiceReturns.FirstOrDefault(ir => ir.InvoiceReturnID == invoiceReturn.InvoiceReturnID);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(invoiceReturn);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Invoice Return updated successfully";
                    response.currentId = invoiceReturn.InvoiceReturnID;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Invoice Return not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Invoice Return: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeleteInvoiceReturn(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.invoiceReturns.FirstOrDefault(ir => ir.InvoiceReturnID == id);
                if (existing != null)
                {
                    existing.IsDeleted = true;
                    _context.Update(existing);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Invoice Return deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete Invoice Return: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
