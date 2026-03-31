namespace FinanceTracker.Contracts.Budgets;

public class UpsertBudgetRequest
{
    public Guid UserId { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal LimitAmount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}
