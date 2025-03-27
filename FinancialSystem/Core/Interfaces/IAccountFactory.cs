using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces;

public interface IAccountFactory
{
    public AccountBase CreateAccount(Bank bank, decimal initialBalance = 0);
}