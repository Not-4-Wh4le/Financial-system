namespace FinancialSystem.Core.Entities;

public abstract class AccountBase
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public Bank Bank { get; set; } = null!;
    public bool IsFrozen { get; set; } = false;
}