namespace FinancialSystem.Core.Enums;

public enum LoanStatus
{
    Pending,    // На рассмотрении
    Approved,   // Одобрен
    Rejected,   // Отклонен
    Active,     // Активный (выплачивается)
    Completed,  // Полностью погашен
    Defaulted   // Просрочен
}
