using GuardiansExpress.Models;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;

public class BillSubmissionReportRepository : IBillSubmissionReportRepository
{
    private readonly MyDbContext _context;

    public BillSubmissionReportRepository(MyDbContext context)
    {
        _context = context;
    }

    public List<BillSubmissionReportDTO> GetBillSubmissionReport()
    {
        var reportData = (from b in _context.BilSubmissions
                          join submittedBy in _context.users on b.BillSubmissionBy equals submittedBy.UserId.ToString() into sb
                          from submittedBy in sb.DefaultIfEmpty()

                          join receivedBy in _context.GroupHeads on b.ReceivedBy equals receivedBy.Id.ToString() into rb
                          from receivedBy in rb.DefaultIfEmpty()

                          join handedOverBy in _context.GroupHeads on b.HandedOverBy equals handedOverBy.Id.ToString() into hob
                          from handedOverBy in hob.DefaultIfEmpty()

                          join courier in _context.countryEntities on b.CourierName equals courier.Id.ToString() into cr
                          from courier in cr.DefaultIfEmpty()

                          where b.IsDelete == false
                          select new BillSubmissionReportDTO
                          {
                              BillSubmissionId = b.BillSubmissionId,
                              ClientName = b.ClientName,
                              BillNo = b.BillNo,
                              BillSubDate = b.BillSubDate,
                              BillSubmissionBy = submittedBy != null ? submittedBy.UserName : null,
                              ReceivedBy = receivedBy != null ? receivedBy.GroupHeadName : null,
                              HandedOverBy = handedOverBy != null ? handedOverBy.GroupHeadName : null,
                              DocketNo = b.DocketNo,
                              CourierName = courier != null ? courier.CountryName : null,
                              IsActive = b.IsActive==false
                          }).ToList();

        return reportData;
    }
}
