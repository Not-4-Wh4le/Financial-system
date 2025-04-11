using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

//работа с предприятиями и зарплатными проектами
public interface IEnterpriseRepository : IRepository<Enterprise>
{
    Task<List<AccountBase>> GetEnterpriseAccountsAsync(int enterpriseId);
    Task CreateSalaryProjectAsync(int enterpriseId, List<int> employeeIds);
    Task AddEmployeeToSalaryProjectAsync(int enterpriseId, int employeeId);
}