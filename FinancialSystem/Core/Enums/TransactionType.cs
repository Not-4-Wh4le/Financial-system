namespace FinancialSystem.Core.Enums;

public enum TransactionType
{
    Deposit,    // Внесение средств
    Withdrawal, // Снятие средств
    Transfer,   // Перевод между счетами
    Interest,   // Начисление процентов
    Fee,        // Комиссия
    Salary     // Зарплата 
}