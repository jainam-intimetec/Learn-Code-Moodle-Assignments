namespace FinanceTracker.Contracts.Budgets;

public class BudgetLookupRequest
{
    public Guid UserId { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
}
