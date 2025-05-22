using GuardiansExpress.Models.DTOs;


namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IDayBookRepository
    {
        List<DayBookDTO> GetDayBookEntries(DateTime startDate, DateTime endDate,
            string branch, string transactionType, string accHead, string balType, string bookType);
    }
}
