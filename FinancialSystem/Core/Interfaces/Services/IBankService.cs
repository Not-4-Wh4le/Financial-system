using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Services;

public interface IBankService
{
    Task RegisterClientAsync(User user, User newUser);
    Task RegisterEnterpriseAsync(User user, Enterprise enterprise);
    Task<Bank> GetBankInfoAsync(int bankId);
    Task AssignAccountToBankAsync(int bankId, int accountId);
}