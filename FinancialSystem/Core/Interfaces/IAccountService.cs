using FinancialSystem.Core.Entities;

namespace FinancialSystem.Core.Interfaces;

public interface IAccountService
{
    // Пополнение счета на сумму
    void Replenishment(AccountBase account, decimal amount);
    
    // Перевод средств со счета на счет
    void Transfer(AccountBase from, AccountBase to, decimal amount);
    
    // Снятие средств со счета
    void Withdraw(AccountBase account, decimal amount);
}