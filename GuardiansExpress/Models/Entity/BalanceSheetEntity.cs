public class BalanceSheetEntity
{
    public int Id { get; set; }
    public string? AccountName { get; set; }
    public string? AccountGroup { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? Balance { get; set; }
    public string? BalanceType { get; set; }
}
