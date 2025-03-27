namespace FinancialSystem.Core.Entities;

public class UserAccount : AccountBase
{
    public User Owner { get; set; } = null!;
}