namespace GuardiansExpress.Models.DTOs
{
    public class BankRecoSummaryDTO
    {
        public string? BankName { get; set; }
        public DateTime OnDate { get; set; }
        public decimal BalanceAsPerBooks { get; set; }
        public decimal DepositedButNotCleared { get; set; }
        public decimal IssuedButNotCleared { get; set; }
        public decimal BalanceAsPerBank { get; set; }
    }
}

