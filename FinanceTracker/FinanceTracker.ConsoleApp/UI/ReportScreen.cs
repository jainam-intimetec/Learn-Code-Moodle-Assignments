using FinanceTracker.Contracts.Auth;
using FinanceTracker.Contracts.Reports;
using Shared.Helpers;

namespace FinanceTracker.ConsoleApp.UI;

public class ReportScreen
{
    private readonly ApiClient _client;

    public ReportScreen(ApiClient client)
    {
        _client = client;
    }

    public async Task ShowAsync(UserResponse user)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("Reports");
            var choice = ConsoleHelper.PromptMenu("Choose an option:", "Current month summary", "Custom month summary", "Back");

            try
            {
                switch (choice)
                {
                    case 1:
                        await PrintReportAsync(new MonthlyReportRequest
                        {
                            UserId = user.Id,
                            Year = DateTime.UtcNow.Year,
                            Month = DateTime.UtcNow.Month
                        });
                        ConsoleHelper.Pause();
                        break;
                    case 2:
                        await PrintReportAsync(new MonthlyReportRequest
                        {
                            UserId = user.Id,
                            Year = ConsoleHelper.PromptInt("Year", DateTime.UtcNow.Year - 5, DateTime.UtcNow.Year + 5),
                            Month = ConsoleHelper.PromptInt("Month", 1, 12)
                        });
                        ConsoleHelper.Pause();
                        break;
                    case 3:
                        return;
                }
            }
            catch (InvalidOperationException ex)
            {
                ConsoleHelper.WriteError(ex.Message);
                ConsoleHelper.Pause();
            }
        }
    }

    private async Task PrintReportAsync(MonthlyReportRequest request)
    {
        var report = await _client.GetMonthlyReportAsync(request);

        ConsoleHelper.WriteHeader($"Report {report.Year}-{report.Month:00}");
        Console.WriteLine($"Transactions : {report.TransactionCount}");
        Console.WriteLine($"Income       : {report.TotalIncome:C}");
        Console.WriteLine($"Expense      : {report.TotalExpense:C}");
        Console.WriteLine($"Savings      : {report.Savings:C}");
        Console.WriteLine();
        Console.WriteLine("Budget Performance");

        if (report.Budgets.Count == 0)
        {
            ConsoleHelper.WriteInfo("No budgets configured for this month.");
            return;
        }

        foreach (var budget in report.Budgets)
        {
            Console.WriteLine($"{budget.Category,-15} | Budget: {budget.BudgetAmount,10:C} | Spent: {budget.SpentAmount,10:C} | Remaining: {budget.RemainingAmount,10:C} | {(budget.IsOverBudget ? "Over budget" : "Within budget")}");
        }
    }
}
