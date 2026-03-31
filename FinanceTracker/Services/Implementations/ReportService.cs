using App.Services.Interfaces;
using Domain.Entities;
using FinanceTracker.Contracts.Reports;
using FinanceTracker.Domain.Enums;
using Infrastructure.Repositories.Interfaces;
using Shared.Exceptions;

namespace App.Services.Implementations;

public class ReportService : IReportService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;

    public ReportService(ITransactionRepository transactionRepository, IBudgetRepository budgetRepository)
    {
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
    }

    public async Task<MonthlyReport> GetMonthlyReportAsync(MonthlyReportRequest request)
    {
        ValidatePeriod(request);

        var transactions = await _transactionRepository.GetByUserIdAsync(request.UserId);
        var monthlyTransactions = transactions
            .Where(t => t.DateUtc.Year == request.Year && t.DateUtc.Month == request.Month)
            .ToList();

        return new MonthlyReport
        {
            Year = request.Year,
            Month = request.Month,
            TransactionCount = monthlyTransactions.Count,
            TotalIncome = monthlyTransactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
            TotalExpense = monthlyTransactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
        };
    }

    public async Task<IReadOnlyList<BudgetPerformance>> GetBudgetPerformanceAsync(MonthlyReportRequest request)
    {
        ValidatePeriod(request);

        var budgets = await _budgetRepository.GetByUserIdAsync(request.UserId);
        var transactions = await _transactionRepository.GetByUserIdAsync(request.UserId);

        var monthBudgets = budgets.Where(b => b.Year == request.Year && b.Month == request.Month).ToList();

        return monthBudgets
            .Select(budget => new BudgetPerformance
            {
                Category = budget.Category,
                BudgetAmount = budget.LimitAmount,
                SpentAmount = transactions
                    .Where(t =>
                        t.Type == TransactionType.Expense &&
                        t.DateUtc.Year == request.Year &&
                        t.DateUtc.Month == request.Month &&
                        string.Equals(t.Category, budget.Category, StringComparison.OrdinalIgnoreCase))
                    .Sum(t => t.Amount)
            })
            .OrderBy(x => x.Category)
            .ToList();
    }

    private static void ValidatePeriod(MonthlyReportRequest request)
    {
        if (request.UserId == Guid.Empty)
            throw new ValidationException("User is required.");

        if (request.Month < 1 || request.Month > 12)
            throw new ValidationException("Month must be between 1 and 12.");

        if (request.Year < DateTime.UtcNow.Year - 5 || request.Year > DateTime.UtcNow.Year + 5)
            throw new ValidationException("Year is out of allowed range.");
    }
}
