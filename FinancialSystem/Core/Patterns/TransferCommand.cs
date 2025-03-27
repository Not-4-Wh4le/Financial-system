using FinancialSystem.Core.Entities;
using FinancialSystem.Core.Interfaces;

namespace FinancialSystem.Core.Patterns;

public class TransferCommand : ITransactionCommand
{
    private readonly AccountBase _from;
    private readonly AccountBase _to;
    private readonly decimal _amount;

    public TransferCommand(AccountBase from, AccountBase to, decimal amount)
    {
        _from = from;
        _to = to;
        _amount = amount;
    }
    
    
    // Выполнить
    public void Execute()
    {
        _from.Balance -= _amount;
        _to.Balance += _amount;
    }
    
    //Отменить
    public void Undo()
    {
        _from.Balance += _amount;
        _to.Balance -= _amount;
    }
}