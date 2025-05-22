using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class InvoiceTypeRepo : IInvoiceTypeRepo
    {
        private readonly MyDbContext _context;

        public InvoiceTypeRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<InvoiceTypeModel> GetInvoiceTypes()
        {
            return _context.invoice
                .Where(it => it.IsDeleted==false)
                .Select(it => new InvoiceTypeModel
                {
                    Id = it.Id,
                    InvoiceType = it.InvoiceType,
                    Status = it.Status,
                    IsDeleted = it.IsDeleted,
                    IsActive = it.IsActive,
                    CreatedBy = it.CreatedBy,
                    CreatedOn = it.CreatedOn,
                    LastUpdatedBy = it.LastUpdatedBy,
                    LastUpdatedOn = it.LastUpdatedOn
                }).ToList();
        }

        public InvoiceTypeModel GetInvoiceTypeById(int id)
        {
            var invoiceType = _context.invoice.Find(id);
            if (invoiceType == null || invoiceType.IsDeleted==false)
                return null;

            return new InvoiceTypeModel
            {
                Id = invoiceType.Id,
                InvoiceType = invoiceType.InvoiceType,
                Status = invoiceType.Status,
                IsDeleted = invoiceType.IsDeleted,
                IsActive = invoiceType.IsActive,
                CreatedBy = invoiceType.CreatedBy,
                CreatedOn = invoiceType.CreatedOn,
                LastUpdatedBy = invoiceType.LastUpdatedBy,
                LastUpdatedOn = invoiceType.LastUpdatedOn
            };
        }

        public GenericResponse CreateInvoiceType(InvoiceTypeMasterEntity invoiceType)
        {
            GenericResponse response = new GenericResponse();
            InvoiceTypeMasterEntity iv = new InvoiceTypeMasterEntity();
            try
            {
                iv.InvoiceType = invoiceType.InvoiceType;
                iv.Status = invoiceType.Status;
                iv.IsDeleted = false;
                iv.IsActive = true;
                _context.invoice.Add(iv);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Invoice Type created successfully";
                response.currentId = invoiceType.Id;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save Invoice Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse UpdateInvoiceType(InvoiceTypeMasterEntity invoiceType)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                var existing = _context.invoice.Find(invoiceType.Id);
                if (existing != null )
                { existing.Id = invoiceType.Id;
                    existing.InvoiceType = invoiceType.InvoiceType;
                    existing.Status = invoiceType.Status;
                    existing.IsDeleted = false;
                    existing.IsActive = true;
                    existing.LastUpdatedOn = DateTime.Now;
                    _context.invoice.Update(existing);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Invoice Type updated successfully";
                    response.currentId = invoiceType.Id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Invoice Type not found";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Invoice Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GenericResponse DeleteInvoiceType(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existing = _context.invoice.Find(id);
                if (existing != null && existing.IsDeleted==false)
                {
                    existing.IsDeleted = true;
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Invoice Type deleted successfully";
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
                response.message = "Failed to delete Invoice Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}
