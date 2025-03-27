using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Patterns;

public class EnterpriseAccountFactory : IAccountFactory
{
    private readonly Enterprise _owner;

    public EnterpriseAccountFactory(Enterprise owner)
    {
        _owner = owner;
    }

    public AccountBase CreateAccount(Bank bank, decimal initialBalance = 0)
    {
        return new EnterpriseAccount
        {
            EnterpriseOwner = _owner,
            Balance = initialBalance,
            Bank = bank,
        };
    }
}