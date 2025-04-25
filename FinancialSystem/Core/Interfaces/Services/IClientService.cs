using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Services;

public interface IClientService
{
    Task<Client> RegisterClientAsync(User executor, string fullName, string passport, string phone, int bankId);
    Task FreezeClientAccountAsync(User executor, int accountId);
    Task RequestLoanAsync(User executor, decimal amount, int months);
    Task<List<AccountBase>> GetClientAccountsAsync(User executor, int clientId);
}