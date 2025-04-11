using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces.Repository;
using FinancialSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Repositories;

public class LoanRepository : GenericRepository<Loan>, ILoanRepository
{
    public LoanRepository(AppDbContext context) : base(context) { }

    public async Task<List<Loan>> GetLoansByClientAsync(int clientId)
    {
        return await _context.Loans
            .Where(l => l.Client.Id == clientId)
            .ToListAsync();
    }

    public async Task<List<Loan>> GetPendingLoansAsync()
    {
        return await _context.Loans
            .Where(l => !l.IsApproved)
            .ToListAsync();
    }
    
    public async Task ApproveLoanAsync(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan != null)
        {
            loan.IsApproved = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RejectLoanAsync(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan != null)
        {
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }
    }
}