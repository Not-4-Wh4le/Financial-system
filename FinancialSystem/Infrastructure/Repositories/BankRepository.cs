using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Repositories;

public class BankRepository : GenericRepository<Bank>, IBankRepository
{
    public BankRepository(AppDbContext context) : base(context) { }

    public async Task<List<IBankClient>> GetClientsByBankAsync(int bankId)
    {
        var bank = await _context.Banks
            .Include(b => b.ClientUsers)
            .Include(b => b.ClientEnterprises)
            .FirstOrDefaultAsync(b => b.Id == bankId);

        if (bank == null)
            return new List<IBankClient>();

        var clients = bank.ClientUsers.Cast<IBankClient>()
                          .Concat(bank.ClientEnterprises)
                          .ToList();

        return clients;
    }

    public async Task AddClientToBankAsync(int bankId, int clientId)
    {
        var bank = await _context.Banks.FindAsync(bankId);
        var user = await _context.Users.FindAsync(clientId);

        if (bank != null && user != null)
        {
            bank.ClientUsers.Add(user);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task AddEnterpriseToBankAsync(int bankId, int enterpriseId)
    {
        var bank = await _context.Banks.FindAsync(bankId);
        var enterprise = await _context.Enterprises.FindAsync(enterpriseId);

        if (bank != null && enterprise != null)
        {
            bank.ClientEnterprises.Add(enterprise);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveClientFromBankAsync(int bankId, int clientId)
    {
        var bank = await _context.Banks
            .Include(b => b.ClientUsers)
            .FirstOrDefaultAsync(b => b.Id == bankId);

        if (bank != null)
        {
            var user = bank.ClientUsers.FirstOrDefault(u => u.Id == clientId);
            if (user != null)
            {
                bank.ClientUsers.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task<Bank?> GetDefaultBankAsync()
    {
        return await _context.Banks.FirstOrDefaultAsync(); 
    }
}