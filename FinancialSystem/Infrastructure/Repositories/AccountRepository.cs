using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Repositories;

public class AccountRepository : GenericRepository<AccountBase>, IAccountRepository
{
    public AccountRepository(AppDbContext context) : base(context) { }

    public async Task<List<AccountBase>> GetAccountsByUserAsync(int userId)
    {
        return await _context.Accounts
            .OfType<UserAccount>()
            .Where(a => a.Owner.Id == userId)
            .Cast<AccountBase>()
            .ToListAsync();
    }

   
    public async Task FreezeAccountAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account != null)
        {
            account.IsFrozen = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UnfreezeAccountAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account != null)
        {
            account.IsFrozen = false;
            await _context.SaveChangesAsync();
        }
    }

    public Task<UserAccount?> GetSalaryAccountAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AccountBase>> GetAccountsByOwnerAsync(int ownerId)
    {
        var userAcc = await GetAccountsByUserAsync(ownerId);
        List<AccountBase> enterpriseAcc = new ();
        enterpriseAcc.Clear();
        return userAcc.Concat(enterpriseAcc).ToList();
    }

   
}