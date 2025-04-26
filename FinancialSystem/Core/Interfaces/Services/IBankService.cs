using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Services;

public interface IBankService
{
    Task RegisterClientAsync(User executor, int bankId, int userId);
    Task RegisterEnterpriseAsync(User executor, int bankId, int enterpriseId);
    Task<Bank> GetBankInfoAsync(int bankId);
    Task AssignAccountToBankAsync(int bankId, int accountId);
    Task<List<Bank>> GetAllBanksAsync(User executor);
    Task<Bank> CreateBankAsync(User executor, Bank bank);
    Task<List<IBankClient>> GetBankClientsAsync(User executor, int bankId);
}