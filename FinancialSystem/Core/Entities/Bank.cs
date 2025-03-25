namespace FinancialSystem.Core.Entities;

public class Bank
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BIC { get; set; } = string.Empty;
    public List<Client> Clients { get; set; } = new();
    public List<Enterprise> Enterprises { get; set; } = new();
    
}