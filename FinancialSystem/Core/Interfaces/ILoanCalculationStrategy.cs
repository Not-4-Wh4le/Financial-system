namespace FinancialSystem.Core.Interfaces;

public interface ILoanCalculationStrategy
{
    decimal Calculate (decimal amount, int months);
}