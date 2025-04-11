using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<List<Transaction>> GetTransactionsByAccountAsync(int accountId);
    Task<List<Transaction>> GetLastTwoTransactionsAsync(int accountId);
    Task CancelTransactionAsync(int transactionId);
    Task<decimal> GetAccountBalanceAsync(int accountId);
}