using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Patterns;

public class FixedRateLoanStrategy : ILoanCalculationStrategy
{
    private readonly decimal  _interestRate;

    // фикс ставка
    public FixedRateLoanStrategy(decimal interestRate) =>
        _interestRate = interestRate;

    public decimal Calculate(decimal amount, int months) =>
        (amount * _interestRate / 12) / (1 - (decimal)Math.Pow(1 + (double)(_interestRate / 12), -months));
    
}