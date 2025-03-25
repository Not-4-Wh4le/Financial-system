using FinancialSystem.Core.Enums;

namespace FinancialSystem.Core.Entities;

public class Transaction
{
    public int Id { get; set; }
    public Account FromAccount { get; set; } = null!;
    public Account ToAccount { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

}