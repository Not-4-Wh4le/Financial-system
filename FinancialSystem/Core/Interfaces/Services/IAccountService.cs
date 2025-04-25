using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Servicec;

public interface IAccountService
{
    Task<decimal> GetBalanceAsync(User user,int accountId);
    Task DepositAsync(User user, int accountId, decimal amount);
    Task WithdrawAsync(User user, int accountId, decimal amount);
    Task TransferAsync(User user, int fromAccountId, int toAccountId, decimal amount);
    Task FreezeAccountAsync(User user, int accountId);
    Task UnfreezeAccountAsync(User user, int accountId);
}