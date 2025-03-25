namespace FinancialSystem.Core.Entities;

public class Loan
{
    public int Id { get; set; }
    public Client Client { get; set; } = null!;
    public decimal Amount { get; set; }
    public int DurationMonths { get; set; }
    public bool IsApproved { get; set; } = false;

}