using FinancialSystem.Core.Enums;

namespace FinancialSystem.Core.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int? FromAccountId { get; set; }
    public AccountBase FromAccountBase { get; set; } = null!;
    public int? ToAccountId { get; set; }
    public AccountBase ToAccountBase { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

}