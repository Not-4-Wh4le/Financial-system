using FinancialSystem.Core.Entities;
using FinancialSystem.Infrastructure.Services;

namespace FinancialSystem.Core.Interfaces.Services;

public interface IAuthService
{
    User? CurrentUser { get; }
    Task<bool> LoginAsync(string email, string password);
    void Logout();
    bool HasPermission(Permission permission);
}