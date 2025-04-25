using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Entities;

public class User : IBankClient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsForeigner { get; set; } = false;
    public List<AccountBase> Accounts { get; set; } = new();
    public List<EmployeeEnterprise> EmployedAt { get; set; } = new();
    public List<Bank> Banks { get; set; } = new();
}