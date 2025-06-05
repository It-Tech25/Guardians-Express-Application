using System.Collections.Generic;

namespace GuardiansExpress.Models.DTOs
{
    public class ProfitAndLossReportDTO
    {
        public List<ProfitAndLossItemDTO> ExpenseItems { get; set; } = new List<ProfitAndLossItemDTO>();
        public List<ProfitAndLossItemDTO> IncomeItems { get; set; } = new List<ProfitAndLossItemDTO>();
        public decimal TotalExpenses { get; set; }
        public decimal TotalIncomes { get; set; }
        public decimal NetProfit { get; set; }
        public decimal NetLoss { get; set; }
    }
}