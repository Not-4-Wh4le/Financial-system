using FinancialSystem.Core.Entities;
using FinancialSystem.Infrastructure.Services;

namespace FinancialSystem.Core.Interfaces.Services;

public interface IAuthorizationService
{
    bool CheckPermission(User user, Permission requiredPermission);
}