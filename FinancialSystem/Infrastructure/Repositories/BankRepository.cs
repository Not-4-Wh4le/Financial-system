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
            .Include(b => b.Clients)
            .FirstOrDefaultAsync(b => b.Id == bankId);

        return bank?.Clients.ToList() ?? new List<IBankClient>();
    }

    public async Task AddClientToBankAsync(int bankId, IBankClient client)
    {
        var bank = await _context.Banks.FindAsync(bankId);
        if (bank != null)
        {
            bank.Clients.Add(client);
            await _context.SaveChangesAsync();
        }
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
}