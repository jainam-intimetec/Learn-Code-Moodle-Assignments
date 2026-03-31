using Domain.Entities;
using FinanceTracker.Contracts.Reports;

namespace App.Services.Interfaces;

public interface IReportService
{
    Task<MonthlyReport> GetMonthlyReportAsync(MonthlyReportRequest request);
    Task<IReadOnlyList<BudgetPerformance>> GetBudgetPerformanceAsync(MonthlyReportRequest request);
}
