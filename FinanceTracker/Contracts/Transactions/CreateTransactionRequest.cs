using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Contracts.Transactions;

public class CreateTransactionRequest
{
    public Guid UserId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime Date { get; set; }
}
