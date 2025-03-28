using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Enums;

namespace FinancialSystem.Core.Interfaces;

public interface ITransactionService
{
    // Обрабатывает финансовую транзакцию.
    TransactionResult ProcessTransaction(Transaction transaction, User initiator);
    
    // Отменяет транзакцию по идентификатору.
    // Клиент не может отменить транзакцию самостоятельно.
    // Операторы могут отменять одну транзакцию, менеджеры - транзакции специалистов сторонних предприятий,
    // администратор может отменять любые действия.
    void CancelTransaction(int transactionId, User canceller);
    
    // история транзакций 
    IReadOnlyList<Transaction> GetTransactionHistory(int accountId);
}