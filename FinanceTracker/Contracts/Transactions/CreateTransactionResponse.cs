namespace FinanceTracker.Contracts.Transactions;

public class CreateTransactionResponse
{
    public string Message { get; set; } = "Transaction saved.";
    public string? BudgetAlertMessage { get; set; }
}
