using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

public interface IAccountRepository : IRepository<AccountBase>
{
    Task<List<AccountBase>> GetAccountsByUserAsync(int userId);
   
    Task FreezeAccountAsync(int accountId);
    Task UnfreezeAccountAsync(int accountId);
    Task<UserAccount?> GetSalaryAccountAsync(int userId);
    
    Task<List<AccountBase>> GetAccountsByOwnerAsync(int ownerId);
    
    

}