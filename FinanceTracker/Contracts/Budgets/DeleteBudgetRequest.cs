namespace FinanceTracker.Contracts.Budgets;

public class DeleteBudgetRequest
{
    public Guid UserId { get; set; }
    public Guid BudgetId { get; set; }
}
