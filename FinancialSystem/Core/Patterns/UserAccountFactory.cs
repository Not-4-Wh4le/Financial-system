using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Patterns;

public class UserAccountFactory : IAccountFactory
{
    private readonly User _owner;

    public UserAccountFactory(User owner)
    {
        _owner = owner;
    }
    
    public AccountBase CreateAccount(Bank bank, decimal initialBalance = 0)
    {
        return new UserAccount
        {
            Owner = _owner,
            Balance = initialBalance,
            Bank = bank
        };
    }
}