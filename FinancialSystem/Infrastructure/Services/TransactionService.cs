using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;



public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthorizationService _authorizationService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IAuthorizationService authorizationService
        )
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _authorizationService = authorizationService;
    }

    public async Task LogTransactionAsync(User user, int? fromAccountId, int? toAccountId, decimal amount, 
        TransactionType type, string message = "log")
    {
        if (!_authorizationService.CheckPermission(user, Permission.ManageOwnAccounts))
            throw new UnauthorizedAccessException("Недостаточно прав для выполнения операции");

        var transaction = new Transaction
        {
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount,
            Type = type,
            Date = DateTime.UtcNow,
            Status = TransactionStatus.Completed,
            Message = message
        };

        await _transactionRepository.AddAsync(transaction);
    }

    public async Task CancelTransactionAsync(User user, int transactionId)
    {
        bool canCancel = _authorizationService.CheckPermission(user, Permission.CancelTransaction) || 
                         _authorizationService.CheckPermission(user, Permission.CancelAnyOperation);
        
        if (!canCancel)
            throw new UnauthorizedAccessException("Недостаточно прав для отмены транзакции");
        
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);

        if (transaction.Status == TransactionStatus.Canceled)
            throw new Exception();//TransactionAlreadyCanceledException(transactionId);

        // Возвращаем средства
        if (transaction.FromAccountId.HasValue)
        {
            var account = await _accountRepository.GetByIdAsync(transaction.FromAccountId.Value);
            account.Balance += transaction.Amount;
            await _accountRepository.UpdateAsync(account);
        }

        transaction.Status = TransactionStatus.Canceled;
        await _transactionRepository.UpdateAsync(transaction);
    }

    public async Task<List<Transaction>> GetAccountTransactionsAsync(int accountId)
    {
        return await _transactionRepository.GetTransactionsByAccountAsync(accountId);
    }

    public async Task<List<Transaction>> GetLastTwoTransactionsAsync(int accountId)
    {
        return await _transactionRepository.GetLastTwoTransactionsAsync(accountId);
    }
}