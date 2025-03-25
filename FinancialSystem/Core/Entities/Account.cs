namespace FinancialSystem.Core.Entities;

public class Account
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public Bank Bank { get; set; } = null!;
    public User? Owner { get; set; }
    public Enterprise? EnterpriseOwner { get; set; }
    public bool IsFrozen { get; set; } = false;
}