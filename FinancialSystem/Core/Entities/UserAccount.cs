using FinancialSystem.Core.Enums;

namespace FinancialSystem.Core.Entities;
//счет физ. лица
public class UserAccount : AccountBase
{
    public User Owner { get; set; } = null!;
    public AccountType AccountType { get; set; } = AccountType.Regular;
}