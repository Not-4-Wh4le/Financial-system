using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Entities;

public class Client :  IBankClient
{
    public int Id { get; set; }
    public User User { get; set; } = null!;

    public string Name
    {
        get => User.Name;
        set => User.Name = value;
    }
}