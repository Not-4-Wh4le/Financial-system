using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;
//управление пользователями (клиентами, сотрудниками).
public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<bool> IsForeignerAsync(int userId);
    Task<List<User>> SearchUsersAsync(string searchTerm);
    Task<List<User>> GetEmployeesByEnterpriseAsync(int enterpriseId);
}