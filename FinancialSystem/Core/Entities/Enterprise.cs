using FinancialSystem.Core.Enums;

namespace FinancialSystem.Core.Entities;

public class Enterprise
{
    public int Id { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string LegalAddress { get; set; } = string.Empty;
    public EnterpriseType Type { get; set; }
    public string UNP { get; set; } = string.Empty;
    public string BIC { get; set; } = string.Empty;
    public Bank Bank { get; set; } = null!;
    public List<Account> Accounts { get; set; } = new();

}