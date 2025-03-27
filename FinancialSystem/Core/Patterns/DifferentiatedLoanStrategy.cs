using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Patterns;

public class DifferentiatedLoanStrategy : ILoanCalculationStrategy
{
    // Дифф ставка
    public decimal Calculate(decimal amount, int months) =>
        (amount / months) + (amount * 0.01m);

}