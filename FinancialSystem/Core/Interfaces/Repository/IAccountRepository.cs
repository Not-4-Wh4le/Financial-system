using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

public interface IAccountRepository
{
    Task<AccountBase> GetAccountByIdAsync(int id);
    Task<List<AccountBase>> GetAccountsByUserAsync(int userId);
    Task AddAccountAsync(AccountBase account);
    Task UpdateAccountAsync(AccountBase account);
    Task FreezeAccountAsync(int accountId);
}