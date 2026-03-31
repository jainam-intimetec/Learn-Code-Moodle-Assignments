namespace FinanceTracker.Contracts.Reports;

public class MonthlyReportRequest
{
    public Guid UserId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}
