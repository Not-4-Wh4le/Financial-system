using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

public interface IBankRepository : IRepository<Bank>
{
    Task<List<IBankClient>> GetClientsByBankAsync(int bankId);
    Task AddClientToBankAsync(int bankId, int clientId);
    Task RemoveClientFromBankAsync(int bankId, int clientId);
    Task<Bank?> GetDefaultBankAsync();
    Task AddEnterpriseToBankAsync(int bankId, int enterpriseId);
    Task<Bank?> GetByIdWithClientsAsync(int bankId);

}