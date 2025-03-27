namespace FinancialSystem.Core.Entities;

public class EnterpriseAccount : AccountBase
{
    public Enterprise EnterpriseOwner { get; set; } = null!;
}