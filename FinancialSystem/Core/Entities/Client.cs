namespace FinancialSystem.Core.Entities;

public class Client : User
{
    public List<Bank> Banks { get; set; } = new();
}