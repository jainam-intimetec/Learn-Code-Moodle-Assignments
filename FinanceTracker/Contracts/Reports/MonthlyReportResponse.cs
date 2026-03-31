namespace FinanceTracker.Contracts.Reports;

public class MonthlyReportResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Savings { get; set; }
    public IReadOnlyList<BudgetPerformanceResponse> Budgets { get; set; } = [];
}
