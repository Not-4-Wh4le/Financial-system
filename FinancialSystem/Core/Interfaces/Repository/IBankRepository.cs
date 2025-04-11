using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

public interface IBankRepository : IRepository<Bank>
{
    Task<List<IBankClient>> GetClientsByBankAsync(int bankId);
    Task AddClientToBankAsync(int bankId, IBankClient client);
    Task RemoveClientFromBankAsync(int bankId, int clientId);
}