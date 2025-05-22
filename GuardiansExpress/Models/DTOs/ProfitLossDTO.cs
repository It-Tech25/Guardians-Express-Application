namespace GuardiansExpress.Models.DTOs
{
    public class ProfitLossDTO
    {
        public List<ProfitLossItemDTO> Expenses { get; set; } = new();
        public List<ProfitLossItemDTO> Incomes { get; set; } = new();
        public decimal TotalExpenses { get; set; }
        public decimal TotalIncomes { get; set; }
        public decimal NetLossOrProfit { get; set; } // Negative = Loss, Positive = Profit

    }

    public class ProfitLossItemDTO
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

}

