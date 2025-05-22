using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;

namespace GuardiansExpress.Repositories
{
    public class GRReportRepository : IGRReportRepository
    {
        private readonly MyDbContext _context;

        public GRReportRepository(MyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<GRDTOs> Getgrdetails()
        {
            var query = from gr in _context.grDetails
                        where (gr.IsDeleted == false)

                        select new GRDTOs
                        {
                            GRId = gr.GRId,
                            Branch = gr.Branch,
                            VehicleNo = gr.VehicleNo,
                            OwnedBy = gr.OwnedBy,
                            Grtype = gr.Grtype,
                            GRNo = gr.GRNo,
                            GRDate = gr.GRDate,
                            Consigner = gr.Consigner,
                            Consignee = gr.Consignee,
                            ClientName = gr.ClientName,
                            Transporter = gr.Transporter,
                            StatesFromPlace = _context.placeEntity.Where(a => a.Id == gr.FromPlace).Select(a => a.PlaceName).FirstOrDefault(),
                            States = _context.placeEntity.Where(a => a.Id == gr.ToPlace).Select(a => a.PlaceName).FirstOrDefault(),
                            ItemDescription = gr.ItemDescription,
                            FreightAmount = gr.FreightAmount,
                            GrossWeight = gr.GrossWeight,
                            LoadWeight = gr.LoadWeight,
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


    }
}
