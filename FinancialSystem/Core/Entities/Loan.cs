namespace FinancialSystem.Core.Entities;

public class Loan
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public decimal Amount { get; set; }
    public int DurationMonths { get; set; }
    public bool IsApproved { get; set; } = false;
    
    public decimal MonthlyPayment => Amount / DurationMonths + (Amount * 0.01m);
    public decimal TotalToPay => MonthlyPayment * DurationMonths;


}