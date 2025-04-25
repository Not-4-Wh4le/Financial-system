using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;

namespace FinancialSystem.Core.Interfaces;

public interface ITransactionService
{
    Task LogTransactionAsync(User user, int? fromAccountId, int? toAccountId, decimal amount, TransactionType type);
    Task CancelTransactionAsync(User user, int transactionId);
    Task<List<Transaction>> GetAccountTransactionsAsync(int accountId);
    Task<List<Transaction>> GetLastTwoTransactionsAsync(int accountId);
}