using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Repositories;

public class EnterpriseRepository : GenericRepository<Enterprise>, IEnterpriseRepository
{
    public EnterpriseRepository(AppDbContext context) : base(context) { }

    public async Task<List<AccountBase>> GetEnterpriseAccountsAsync(int enterpriseId)
    {
        return await _context.Accounts
            .OfType<EnterpriseAccount>()
            .Where(a => a.EnterpriseOwner.Id == enterpriseId)
            .Cast<AccountBase>()
            .ToListAsync();
    }
    
    public async Task CreateSalaryProjectAsync(int enterpriseId, List<int> employeeIds)
    {
        var enterprise = await _context.Enterprises.FindAsync(enterpriseId);
        if (enterprise == null) throw new Exception("Enterprise not found");

        var employees = await _context.Users
            .Where(u => employeeIds.Contains(u.Id))
            .ToListAsync();

        foreach (var employee in employees)
        {
            var account = new UserAccount
            {
                Owner = employee,
                Bank = enterprise.Bank,
                Balance = 0
            };
            await _context.Accounts.AddAsync(account);
        }
        await _context.SaveChangesAsync();
    }
    
    public async Task AddEmployeeToSalaryProjectAsync(int enterpriseId, int employeeId)
    {
        var enterprise = await _context.Enterprises.FindAsync(enterpriseId);
        var employee = await _context.Users.FindAsync(employeeId);

        if (enterprise != null && employee != null)
        {
            var account = new UserAccount
            {
                Owner = employee,
                Bank = enterprise.Bank,
                Balance = 0
            };
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<User>> GetEnterpriseEmployeesAsync(int enterpriseId)
    {
        return await _context.EmployeeEnterprises
            .Where(ee => ee.EnterpriseId == enterpriseId)
            .Include(ee => ee.User) 
            .ThenInclude(u => u.Accounts) 
            .Select(ee => ee.User)
            .ToListAsync();
    }

    public Task<Enterprise?> GetByIdWithEmployeesAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<EnterpriseAccount?> GetMainEnterpriseAccountAsync(int enterpriseId)
    {
        return await _context.Enterprises
            .Where(e => e.Id == enterpriseId)
            .SelectMany(e => e.Accounts)
            .OfType<EnterpriseAccount>()
            .FirstOrDefaultAsync(a => a.IsMainAccount);
    }
    
}