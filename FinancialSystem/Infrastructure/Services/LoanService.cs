using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces;
using FinancialSystem.Core.Enums;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Core.Interfaces.Servicec;
using FinancialSystem.Core.Interfaces.Services;

namespace FinancialSystem.Infrastructure.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IAccountService _accountService;
    private readonly ILoanCalculationStrategy _calculationStrategy;
    private readonly IAuthorizationService _authorizationService;

    public LoanService(
        ILoanRepository loanRepository,
        IAccountService accountService,
        ILoanCalculationStrategy calculationStrategy,
        IAuthorizationService authorizationService)
    {
        _loanRepository = loanRepository;
        _accountService = accountService;
        _calculationStrategy = calculationStrategy;
        _authorizationService = authorizationService;
    }

    public async Task<Loan> CreateLoanAsync(User user, int clientId, decimal amount, int months)
    {
        if (!_authorizationService.CheckPermission(user, Permission.RequestLoan))
            throw new UnauthorizedAccessException("Недостаточно прав для запроса кредита");
        
        var monthlyPayment = _calculationStrategy.Calculate(amount, months);
        
        var loan = new Loan
        {
            User = user,
            Amount = amount,
            DurationMonths = months,
            IsApproved = false
        };

        await _loanRepository.AddAsync(loan);
        return loan;
    }

    public async Task ApproveLoanAsync(User user, int loanId)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ApproveLoan))
            throw new UnauthorizedAccessException("Недостаточно прав для одобрения кредита");
        
        await _loanRepository.ApproveLoanAsync(loanId);
        
        // Если нужно зачислить средства на счет при одобрении:
        // var loan = await _loanRepository.GetByIdAsync(loanId);
        // await _accountService.DepositAsync(accountId, loan.Amount);
    }

    public async Task RejectLoanAsync(User user, int loanId)
    {
        if (!_authorizationService.CheckPermission(user, Permission.ApproveLoan))
            throw new UnauthorizedAccessException("Недостаточно прав для отклонения кредита");
        
        await _loanRepository.RejectLoanAsync(loanId);
    }

    public async Task<List<Loan>> GetClientLoansAsync(int clientId)
    {
        return await _loanRepository.GetLoansByClientAsync(clientId);
    }

    public async Task<List<Loan>> GetPendingLoansAsync()
    {
        return await _loanRepository.GetPendingLoansAsync();
    }
}