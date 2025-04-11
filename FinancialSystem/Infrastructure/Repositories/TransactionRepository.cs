using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Infrastructure.Data;
using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context) : base(context) { }

    public async Task<List<Transaction>> GetTransactionsByAccountAsync(int accountId)
    {
        return await _context.Transactions
            .Where(t => t.FromAccountBase.Id == accountId || t.ToAccountBase.Id == accountId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }
    
    public async Task<List<Transaction>> GetLastTwoTransactionsAsync(int accountId)
    {
        return await _context.Transactions
            .Where(t => t.FromAccountBase.Id == accountId || t.ToAccountBase.Id == accountId)
            .OrderByDescending(t => t.Date)
            .Take(2)
            .ToListAsync();
    }
    public async Task CancelTransactionAsync(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction != null)
        {
            transaction.Status = TransactionStatus.Canceled;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        return account?.Balance ?? 0;
    }
}