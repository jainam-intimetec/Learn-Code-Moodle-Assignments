namespace FinanceTracker.Contracts.Reports;

public class BudgetPerformanceResponse
{
    public string Category { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public bool IsOverBudget { get; set; }
}
