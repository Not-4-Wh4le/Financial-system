using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

public interface IClientRepository
{
    Task AddAsync(Client client);
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByPassportAsync(string passportNumber);
    Task UpdateAsync(Client client);
}