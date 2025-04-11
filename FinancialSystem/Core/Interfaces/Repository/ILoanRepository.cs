using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces.Repository;

public interface ILoanRepository : IRepository<Loan>
{
    Task<List<Loan>> GetLoansByClientAsync(int clientId);
    Task<List<Loan>> GetPendingLoansAsync();
    Task ApproveLoanAsync(int loanId);
    Task RejectLoanAsync(int loanId);
}