using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Newtonsoft.Json;

namespace GuardiansExpress.Repositories.Repos
{
    public class FinancialYearRepo: IFinancialRepo
    {

        private readonly MyDbContext _context;

        public FinancialYearRepo(MyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<FinancialyearDto> Get()
        {
            // Query to fetch financial years and related invoices/bills
            var query = from fy in _context.finance
                        where fy.IsDeleted==false // Filter out deleted financial years
                        select new
                        {
                            FinancialYear = fy,
                            Invoices = (from fi in _context.finance
                                        where fi.FinancialYearId == fy.FinancialYearId && fi.IsDeleted==false // Filter out deleted invoices
                                        select fi).ToList(),
                            Bills = (from fb in _context.fBill
                                     where fb.FinancialYearId == fy.FinancialYearId && fb.IsDeleted==false // Filter out deleted bills
                                     select fb).ToList()
                        };

            // Execute the query and fetch results
            var result = query.ToList();

            // Map the results to DTOs
            var financialYearDtos = result
                .Select(item => new FinancialyearDto
                {
                    FinancialYearId = item.FinancialYear.FinancialYearId,
                    FromYear = item.FinancialYear.FromYear,
                    ToYear = item.FinancialYear.ToYear,
                    StartDate = item.FinancialYear.StartDate,
                    EndDate = item.FinancialYear.EndDate,
                    IsDefault = item.FinancialYear.IsDefault,
                    Branch = item.FinancialYear.Branch,
                    IsDeleted = item.FinancialYear.IsDeleted,
                    IsActive = item.FinancialYear.IsActive,
                    CreatedOn = item.FinancialYear.CreatedOn,
                    CreatedBy = item.FinancialYear.CreatedBy,
                    UpdatedOn = item.FinancialYear.UpdatedOn,
                    UpdatedBy = item.FinancialYear.UpdatedBy,
                    InvoiceTypeId = item.FinancialYear.InvoiceTypeId,
                    fiv = item.Invoices
                        .Select(fi => new FinancialInvoiceDto
                        {
                            InvoiceTypeId = fi.InvoiceTypeId,
                            StartDate = Convert.ToDateTime(fi.StartDate),
                            EndDate = Convert.ToDateTime(fi.EndDate),
                            Branch = fi.Branch,
                        }).ToList(),
                    fib = item.Bills
                        .Select(fb => new FinancialBillDto
                        {
                            BillId = fb.BillId,
                            BillTypeId = fb.BillTypeId,
                            BillSeries = fb.BillSeries,
                            StartDate = fb.StartDate,
                            EndDate = fb.EndDate,
                            Branch = fb.Branch
                        }).ToList()
                })
                .ToList();

            return financialYearDtos;
        }
        public GenericResponse Add(FinancialyearDto fy, string serializedInvoiceData, string serializedBillData)
        {
            var invoiceTypes = JsonConvert.DeserializeObject<List<FinancialInvoiceDto>>(serializedInvoiceData);
            var billTypes = JsonConvert.DeserializeObject<List<FinancialBillDto>>(serializedBillData);
            fy.fiv = invoiceTypes;
            fy.fib = billTypes;
            GenericResponse res = new GenericResponse();

            try
            {
                FinancialyearEntity fentity = new FinancialyearEntity
                {
                    FromYear = fy.FromYear,
                    ToYear = fy.ToYear,
                    StartDate = fy.StartDate,
                    EndDate = fy.EndDate,
                    IsDefault = fy.IsDefault,
                    Branch = fy.Branch,
                    IsDeleted = fy.IsDeleted ?? false, 
                    IsActive = fy.IsActive ?? true,  
                    CreatedOn = DateTime.UtcNow,      
                    CreatedBy = fy.CreatedBy ?? 0,   
                    InvoiceTypeId = fy.InvoiceTypeId
                };

                _context.finance.Add(fentity);
                _context.SaveChanges();

                int financialYearId = fentity.FinancialYearId;

                if (invoiceTypes != null && invoiceTypes.Any())
                {
                    foreach (var invoice in invoiceTypes)
                    {
                        FinancialInvoiceEntity fientity = new FinancialInvoiceEntity
                        {
                            FinancialYearId = financialYearId,
                            Branch = invoice.Branch,
                            InvoiceTypeId = invoice.InvoiceTypeId,
                            InvoiceSeries = invoice.InvoiceSeries,
                            StartDate = invoice.StartDate,
                            EndDate = invoice.EndDate,
                            IsDeleted = false,        
                            IsActive = true,         
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = fy.CreatedBy?.ToString() ?? "System"
                        };

                        _context.finvoice.Add(fientity);
                    }
                }

                if (billTypes != null && billTypes.Any())
                {
                    foreach (var bill in billTypes)
                    {
                        FinancialBillEntity fbid = new FinancialBillEntity
                        {
                            FinancialYearId = financialYearId,
                            Branch = bill.Branch,
                            BillTypeId = bill.BillTypeId,
                            BillSeries = bill.BillSeries,
                            StartDate = bill.StartDate,
                            EndDate = bill.EndDate,
                            IsDeleted = false,       
                            IsActive = true,          
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = fy.CreatedBy?.ToString() ?? "System" 
                        };

                        _context.fBill.Add(fbid);
                    }
                }

                _context.SaveChanges();

                res.statuCode = 1;
                res.message = "Financial year and related data added successfully.";
            }
            catch (Exception ex)
            {
                res.statuCode = 0;
                res.message = "An error occurred while adding financial year data: " + ex.Message;
            }

            return res;
        }
        public FinancialyearEntity GetFinancialYearById(int id)
        {
            return _context.finance.Where(a => a.FinancialYearId == id && a.IsDeleted == false).FirstOrDefault();
        }
        public List<FinancialInvoiceEntity> GetFinancialInvoiceById(int id)
        {
            List<FinancialInvoiceEntity> fi = new List<FinancialInvoiceEntity>();
            fi = _context.finvoice.Where(a => a.FinancialYearId == id).ToList();
            return fi;
                }
        public List<FinancialBillEntity> GetFinancialBillById(int id)
        {
            List<FinancialBillEntity> fb = new List<FinancialBillEntity>();
            fb = _context.fBill.Where(a => a.FinancialYearId == id).ToList();
            return fb;
        }

        public async Task<bool> UpdateFinancialYearAsync(FinancialyearDto financialYear, string serializedInvoiceData, string serializedBillData)
        {
            var invoiceTypes = JsonConvert.DeserializeObject<List<FinancialInvoiceDto>>(serializedInvoiceData);
            var billTypes = JsonConvert.DeserializeObject<List<FinancialBillDto>>(serializedBillData);
            // Fetch existing financial year
            var fexist = _context.finance.Where(a => a.FinancialYearId == financialYear.FinancialYearId && a.IsDeleted == false).FirstOrDefault();
               

            if (fexist == null)
            {
                throw new ArgumentException("Financial year not found.");
            }

            // Update financial year properties
            fexist.FromYear = financialYear.FromYear;
            fexist.ToYear = financialYear.ToYear;
            fexist.StartDate = financialYear.StartDate;
            fexist.EndDate = financialYear.EndDate;
            fexist.IsDefault = financialYear.IsDefault;
            fexist.Branch = financialYear.Branch;
            fexist.IsDeleted = financialYear.IsDeleted ?? false;
            fexist.IsActive = financialYear.IsActive ?? true;
            fexist.UpdatedOn = DateTime.UtcNow;
            fexist.UpdatedBy = 1;

            if (invoiceTypes != null && invoiceTypes.Any())
            {
                var existingInvoice = _context.finvoice.Where(a => a.FinancialYearId == fexist.FinancialYearId).ToList();
                foreach(var i in existingInvoice) {
                    var fi = i;
                    foreach (var invoice in invoiceTypes)
                    {
                        if (fi != null)
                        {
                            // Update existing invoice
                            fi.InvoiceSeries = invoice.InvoiceSeries;
                            fi.StartDate = invoice.StartDate;
                            fi.EndDate = invoice.EndDate;
                            fi.IsDeleted = false;
                            fi.IsActive = true;
                            fi.UpdatedOn = DateTime.UtcNow;
                        }
                        break;
                    }
                    _context.finvoice.Update(fi);
                }
            }

            var existingBill = _context.fBill.Where(a => a.FinancialYearId == fexist.FinancialYearId).ToList();
            foreach (var i in existingBill)
            {
                var fb = i;

                foreach (var bill in billTypes)
                {

                    if (fb != null)
                    {
                        fb.BillSeries = bill.BillSeries;
                        fb.StartDate = bill.StartDate;
                        fb.EndDate = bill.EndDate;
                        fb.IsDeleted = false;
                        fb.IsActive = true;
                        fb.UpdatedOn = DateTime.UtcNow;
                    }
                    break;
                }
                _context.fBill.Update(fb);
            }

            _context.finance.Update(fexist);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteFinancialYearAsync(int id)
        {
            var fexist = _context.finance.Where(a => a.FinancialYearId == id && a.IsDeleted == false).FirstOrDefault();


            if (fexist == null)
            {
                throw new ArgumentException("Financial year not found.");
            }


            fexist.IsDeleted = true;
            fexist.IsActive = false;
            fexist.UpdatedOn = DateTime.UtcNow;
            fexist.UpdatedBy = 1;

            _context.finance.Update(fexist);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
