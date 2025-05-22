using Azure;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace GuardiansExpress.Repositories.Repos
{
    public class GRRepo: IGRRepo
    {
        private readonly MyDbContext _context;

        public GRRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GRDTOs> Getgrdetails()
        {
            var query = from gr in _context.grDetails
                        where (gr.IsDeleted==false)

                        select new GRDTOs
                        {
                            GRId = gr.GRId,
                            Branch =gr.Branch,
                            VehicleNo =gr.VehicleNo,
                            OwnedBy = gr.OwnedBy,
                            Grtype =gr.Grtype,
                            GRNo = gr.GRNo,
                            GRDate = gr.GRDate,
                            Consigner = gr.Consigner,
                            Consignee = gr.Consignee,
                            ClientName = gr.ClientName,
                            Transporter = gr.Transporter,
                            StatesFromPlace = _context.placeEntity.Where(a=>a.Id==gr.FromPlace).Select(a=>a.PlaceName).FirstOrDefault(),
                            States = _context.placeEntity.Where(a => a.Id == gr.ToPlace).Select(a => a.PlaceName).FirstOrDefault(),
                            ItemDescription = gr.ItemDescription,
                            FreightAmount = gr.FreightAmount,
                            GrossWeight=gr.GrossWeight,
                            LoadWeight=gr.LoadWeight,
                            HireRate = gr.HireRate,
                            Quantity = gr.Quantity,
                            //InvoiceDate = gr.InvoiceDate,
                            //EWayBillNo = gr.EWayBillNo,
                            //Refno = gr.Refno,
                            //RefDate = gr.RefDate,
                            //RecRefNo = gr.RecRefNo,
                        };

            return query.ToList();  // Executes the query and returns the result
        }

        public GenericResponse AddAsync(GRDTOs grDto, string serializedinvoiceData)
        {
            var invoiceTypes = JsonConvert.DeserializeObject<List<InvoiceGrEntity>>(serializedinvoiceData);
            GenericResponse responce = new GenericResponse();

            int from = _context.stateEntities.Where(a => a.StateName == grDto.StatesFromPlace ).Select(a => a.Id).FirstOrDefault();
            int to = _context.stateEntities.Where(a => a.StateName == grDto.States ).Select(a => a.Id).FirstOrDefault();
            int count = _context.grDetails.Where(a =>
     a.Branch == grDto.Branch &&
     a.VehicleNo == grDto.VehicleNo &&
     a.OwnedBy == grDto.OwnedBy &&
     a.Grtype == grDto.Grtype &&
     a.GRNo == grDto.GRNo &&
     a.GRDate == grDto.GRDate &&
     a.Consigner == grDto.Consigner &&
     a.Consignee == grDto.Consignee &&
     a.ClientName == grDto.ClientName &&
     a.Transporter == grDto.Transporter &&
     a.GrossWeight == grDto.GrossWeight &&
     a.LoadWeight == grDto.LoadWeight &&
     a.FromPlace == (from == 0 ? null : from) &&
     a.ToPlace == (to == 0 ? null : to) &&
     a.ItemDescription == grDto.ItemDescription &&
     a.FreightAmount == grDto.FreightAmount &&
     a.HireRate == grDto.HireRate &&
     a.Quantity == grDto.Quantity &&
     a.LoadType == grDto.LoadType &&
     a.InsurenceNo == grDto.InsurenceNo &&
     a.IncurencedBy == grDto.IncurencedBy
 ).Count();
            if (count < 1)
            {
                try
                {
                    var grEntity = new GREntity
                    {
                        Branch = grDto.Branch,
                        VehicleNo = grDto.VehicleNo,
                        OwnedBy = grDto.OwnedBy,
                        Grtype = grDto.Grtype,
                        GRNo = grDto.GRNo,
                        GRDate = grDto.GRDate,
                        Consigner = grDto.Consigner,
                        Consignee = grDto.Consignee,
                        ClientName = grDto.ClientName,
                        Transporter = grDto.Transporter,
                        GrossWeight = grDto.GrossWeight,
                        LoadWeight = grDto.LoadWeight,
                        FromPlace = from == 0 ? null : from,
                        ToPlace = to == 0 ? null : to,
                        ItemDescription = grDto.ItemDescription,
                        FreightAmount = grDto.FreightAmount,
                        HireRate = grDto.HireRate,
                        Quantity = grDto.Quantity,
                        LoadType = grDto.LoadType,
                        InsurenceNo = grDto.InsurenceNo,
                        IncurencedBy = grDto.IncurencedBy,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedBy = 1
                    };

                    _context.grDetails.Add(grEntity);
                    _context.SaveChanges(); // Save and generate GRId

                    // Use the GRId directly from the entity instead of querying again
                    int id = grEntity.GRId;

                    if (invoiceTypes != null && invoiceTypes.Any())
                    {
                        foreach (var invoice in invoiceTypes)
                        {
                            InvoiceGrEntity fientity = new InvoiceGrEntity
                            {
                                GRId = id,
                                InvoiceNo = invoice.InvoiceNo,
                                InvoiceDate = invoice.InvoiceDate,
                                InvoiceValue = invoice.InvoiceValue,
                                EwayBillNo = invoice.EwayBillNo,
                                EwayBillExpiredate = invoice.EwayBillExpiredate,
                                Isdeleted = false,
                                IsActive = true
                            };

                            _context.invoicegr.Add(fientity);
                        }
                        _context.SaveChanges();
                    }

                    responce.statuCode = 1;
                    responce.currentId = grEntity.GRId;
                    responce.message = "GR Added Successfully";
                }
                catch (Exception ex)
                {
                    responce.statuCode = 0;
                    responce.message = "GR Failed to Add: " + ex.Message;
                }
                return responce;
            }
            else
            {
                responce.statuCode = 1;
                responce.message = "GR Added Successfully";
                return responce;
            }
        }

        // Update an existing GR record
        public GenericResponse UpdateAsync(GRDTOs grDto, string serializedinvoiceData)
        {
            GenericResponse res = new GenericResponse();

            try
            {
                // Deserialize invoice data
                var invoiceTypes = JsonConvert.DeserializeObject<List<InvoiceGrEntity>>(serializedinvoiceData);

                // Fetch the entity to update
                var entity = _context.grDetails
                    .FirstOrDefault(a => a.GRId == grDto.GRId);

                if (entity == null)
                {
                    res.message = "GR record not found.";
                    res.statuCode = 0;
                    return res;
                }

                // Update GR details
                entity.Branch = grDto.Branch;
                entity.VehicleNo = grDto.VehicleNo;
                entity.OwnedBy = grDto.OwnedBy;
                entity.Grtype = grDto.Grtype;
                entity.GRNo = grDto.GRNo;
                entity.GRDate = grDto.GRDate;
                entity.Consigner = grDto.Consigner;
                entity.LoadWeight = grDto.LoadWeight;
                entity.GrossWeight = grDto.GrossWeight;
                entity.Consignee = grDto.Consignee;
                entity.ClientName = grDto.ClientName;
                entity.Transporter = grDto.Transporter;
                entity.FromPlace = _context.placeEntity
                    .Where(a => a.PlaceName == grDto.StatesFromPlace && a.IsDeleted==false)
                    .Select(a => a.Id)
                    .FirstOrDefault();
                entity.ToPlace = _context.placeEntity
                    .Where(a => a.PlaceName == grDto.States && a.IsDeleted==false)
                    .Select(a => a.Id)
                    .FirstOrDefault();
                entity.ItemDescription = grDto.ItemDescription;
                entity.FreightAmount = grDto.FreightAmount;
                entity.LoadType = grDto.LoadType;
                entity.HireRate = grDto.HireRate;
                entity.Quantity = grDto.Quantity;
                entity.UpdatedOn = DateTime.Now;
                entity.UpdatedBy = 1; // Replace with dynamic user ID

                var invoices = _context.invoicegr.Where(a => a.GRId == grDto.GRId).ToList();

                if (invoices != null)
                {
                    foreach (var invoice in invoices)
                    {
                        InvoiceGrEntity fientity = new InvoiceGrEntity
                        {GRId=invoice.GRId,
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceDate = invoice.InvoiceDate,
                            InvoiceValue = invoice.InvoiceValue,
                            EwayBillNo = invoice.EwayBillNo,
                            EwayBillExpiredate = invoice.EwayBillExpiredate,
                            Isdeleted = false,
                            IsActive = true
                        };

                        _context.invoicegr.Update(fientity);
                    }
                }
                // Save changes to the database
                _context.SaveChanges();

                res.message = "Successfully Updated";
                res.currentId = entity.GRId;
                res.statuCode = 1;
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                res.message = $"Update Failed: {ex.Message}";
                res.statuCode = 0;
            }

            return res;
        }

        // Delete an existing GR record
        public GenericResponse DeleteAsync(int id)
        { GenericResponse res = new GenericResponse();
            var entity = _context.grDetails.Where(a => a.GRId == id && a.IsDeleted == false).FirstOrDefault();
            if (entity == null)
            { res.ErrorMessage = "Unable to fetch Record";
                return res;
            }

            entity.IsDeleted = true;
            entity.IsActive = false;
            _context.grDetails.Update(entity);
            _context.SaveChanges();
            res.message = "GR Deleted Successfully";
            res.statuCode = 1;
            return res;
        }
    }
}
