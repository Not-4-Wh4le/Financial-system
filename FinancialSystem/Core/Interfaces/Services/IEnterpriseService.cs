using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;

namespace FinancialSystem.Core.Interfaces.Services;

public interface IEnterpriseService
{
    Task<Enterprise> RegisterEnterpriseAsync(User executor, string legalName, string unp, int bankId);
    Task AddEmployeeAsync(User executor, int enterpriseId, int userId, EnterpriseRole role);
    Task CreateSalaryProjectAsync(User executor, int enterpriseId);
    Task<List<Enterprise>> GetAllEnterprisesAsync(User executor);
    Task PaySalariesAsync(User executor, int enterpriseId, Dictionary<int, decimal> payments);
}