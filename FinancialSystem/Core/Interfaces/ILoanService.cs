using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces;

public interface ILoanService
{
    void RequestLoan(Client client, decimal amount, int months);
    void ApproveLoan(int loanId);
    decimal CalculateMonthlyPayment(decimal amount, decimal interestRate, int months);
    
}