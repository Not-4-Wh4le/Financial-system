namespace FinancialSystem.Core.Entities;
//счет юр лица
public class EnterpriseAccount : AccountBase
{
    public Enterprise EnterpriseOwner { get; set; } = null!;
    public bool IsMainAccount { get; set; }
}