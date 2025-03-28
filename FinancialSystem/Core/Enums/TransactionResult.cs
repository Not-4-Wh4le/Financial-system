namespace FinancialSystem.Core.Enums;

public enum TransactionResult
{
    Success,
    InsufficientFunds,
    AccountFrozen,
    Unauthorized,
    TransactionNotFound
}