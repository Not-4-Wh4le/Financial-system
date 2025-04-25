using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Services;

public interface IUserService
{
    Task<User> RegisterUserAsync(User executor, User newUser);
    Task UpdateUserAsync(User executor, User updatedUser);
    Task<User?> GetUserByIdAsync(User executor, int userId);
    Task DeleteUserAsync(User executor, int userId);
    Task<List<User>> SearchUsersAsync(User executor, string searchTerm);
}