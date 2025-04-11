namespace FinancialSystem.Core.Entities;

public class EmployeeEnterprise
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int EnterpriseId { get; set; }
    public Enterprise Enterprise { get; set; } = null!;
}