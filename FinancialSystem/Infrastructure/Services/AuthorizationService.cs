using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;

public class AuthorizationService : IAuthorizationService
{
    public bool CheckPermission(User user, Permission requiredPermission)
    {
        return user.Role switch
        {
            UserRole.Administrator => true,
            UserRole.Manager => requiredPermission != Permission.ViewAllLogs,
            UserRole.Operator => requiredPermission == Permission.CancelTransaction 
                                 || requiredPermission == Permission.ViewStatistics
                                 || requiredPermission == Permission.ApproveSalaryProject,
            UserRole.Client => requiredPermission == Permission.RequestLoan 
                               || requiredPermission == Permission.ManageOwnAccounts
                               || requiredPermission == Permission.RequestSalaryProject,
            UserRole.EnterpriseSpecialist => requiredPermission == Permission.RequestSalaryProject
                                             || requiredPermission == Permission.ManageSalaryProjects,

            _ => false
        };
    }
}

public enum Permission
{
    ManageOwnAccounts,
    RequestLoan,
    RequestSalaryProject,
    CancelTransaction,
    ApproveLoan,
    ApproveSalaryProject,
    ViewStatistics,
    ViewAllLogs,
    CancelAnyOperation,
    ManageClients,
    ManageEnterprises,
    ManageSalaryProjects
}