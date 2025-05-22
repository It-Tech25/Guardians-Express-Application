using GuardiansExpress.Models.DTOs;

public class BillSubmissionReportService : IBillSubmissionReportService
{
    private readonly IBillSubmissionReportRepository _repository;

    public BillSubmissionReportService(IBillSubmissionReportRepository repository)
    {
        _repository = repository;
    }

    public List<BillSubmissionReportDTO> GetReport()
    {
        return _repository.GetBillSubmissionReport();
    }
}
