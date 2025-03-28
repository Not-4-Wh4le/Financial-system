using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Entities;

public class Client : User, IBankClient
{
    public int Id 
    { 
        get => this.ID; 
        set => this.ID = value; 
    }
    
    public string Name
    { 
        get => this.FullName; 
        set => this.FullName = value; 
    }
    public List<Bank> Banks { get; set; } = new();
}