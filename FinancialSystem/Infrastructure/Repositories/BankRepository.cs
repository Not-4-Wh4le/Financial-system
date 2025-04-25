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
            .FirstOrDefaultAsync(b => b.Id == bankId);
        var users = _context.Entry(bank)
            .Collection("Users")
            .Query()
            .Cast<IBankClient>()
            .ToList();

        var enterprises = _context.Entry(bank)
            .Collection("Enterprises")
            .Query()
            .Cast<IBankClient>()
            .ToList();
        return bank?.Clients.ToList() ?? new List<IBankClient>();
    }

    public async Task AddClientToBankAsync(int bankId, int clientId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "INSERT INTO BankUsers (BankId, UserId) VALUES ({0}, {1})",
            bankId, clientId);
    }
    
    public async Task AddEnterpriseToBankAsync(int bankId, int enterpriseId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "INSERT INTO BankEnterprises (BankId, EnterpriseId) VALUES ({0}, {1})",
            bankId, enterpriseId);
    }

    public async Task RemoveClientFromBankAsync(int bankId, int clientId)
    {
        var bank = await _context.Banks.FindAsync(bankId);
        if (bank != null)
        {
            var client = bank.Clients.FirstOrDefault(c => c.Id == clientId);
            if (client != null)
            {
                bank.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
            
        }
    }
    public async Task<Bank?> GetDefaultBankAsync()
    {
        return await _context.Banks.FirstOrDefaultAsync(); 
    }
}