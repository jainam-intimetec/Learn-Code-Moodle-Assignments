namespace FinanceTracker.Contracts.Transactions;

public class DeleteTransactionRequest
{
    public Guid UserId { get; set; }
    public Guid TransactionId { get; set; }
}
