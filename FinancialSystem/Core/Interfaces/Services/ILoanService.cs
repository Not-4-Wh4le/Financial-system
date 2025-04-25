using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces;

public interface ILoanService
{
    Task<Loan> CreateLoanAsync(User user, int clientId, decimal amount, int months);
    Task ApproveLoanAsync(User user, int loanId);
    Task RejectLoanAsync(User user, int loanId);
    Task<List<Loan>> GetClientLoansAsync(int clientId);
    Task<List<Loan>> GetPendingLoansAsync();
}