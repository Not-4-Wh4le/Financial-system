using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces;

public interface ITransactionService
{
    // Обрабатывает финансовую транзакцию.
    void ProcessTransaction(Transaction transaction);
    
    // Отменяет транзакцию по идентификатору.
    // Клиент не может отменить транзакцию самостоятельно.
    // Операторы могут отменять одну транзакцию, менеджеры - транзакции специалистов сторонних предприятий,
    // администратор может отменять любые действия.
    void CancelTransaction(int transactionId);
}