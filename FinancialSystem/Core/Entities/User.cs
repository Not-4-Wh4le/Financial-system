namespace FinancialSystem.Core.Entities;

public abstract class User
{
    public int ID { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<AccountBase> Accounts { get; set; } = new();

}