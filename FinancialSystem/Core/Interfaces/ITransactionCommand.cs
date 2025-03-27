namespace FinancialSystem.Core.Interfaces;

public interface ITransactionCommand
{
    void Execute();
    void Undo();
}