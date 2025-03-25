using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces;

public interface IAccountService
{
    // Пополнение счета на сумму
    void Replenishment(Account account, decimal amount);
    
    // Перевод средств со счета на счет
    void Transfer(Account from, Account to, decimal amount);
    
    // Снятие средств со счета
    void Withdraw(Account account, decimal amount);
}