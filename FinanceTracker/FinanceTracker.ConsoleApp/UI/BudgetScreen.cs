using FinanceTracker.Contracts.Auth;
using FinanceTracker.Contracts.Budgets;
using Shared.Helpers;

namespace FinanceTracker.ConsoleApp.UI;

public class BudgetScreen
{
    private readonly ApiClient _client;

    public BudgetScreen(ApiClient client)
    {
        _client = client;
    }

    public async Task ShowAsync(UserResponse user)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("Budgets");
            var choice = ConsoleHelper.PromptMenu("Choose an option:", "Create or update", "View all", "Delete", "Back");

            try
            {
                switch (choice)
                {
                    case 1:
                        await _client.SaveBudgetAsync(new UpsertBudgetRequest
                        {
                            UserId = user.Id,
                            Category = ConsoleHelper.PromptRequiredText("Category", 2, 50),
                            Month = ConsoleHelper.PromptInt("Month", 1, 12),
                            Year = ConsoleHelper.PromptInt("Year", DateTime.UtcNow.Year - 5, DateTime.UtcNow.Year + 5),
                            LimitAmount = ConsoleHelper.PromptDecimal("Budget limit", 0.01m, 100000000m)
                        });
                        ConsoleHelper.WriteSuccess("Budget saved.");
                        ConsoleHelper.Pause();
                        break;
                    case 2:
                        var budgets = await _client.GetBudgetsAsync(user.Id);
                        if (budgets.Count == 0)
                        {
                            ConsoleHelper.WriteInfo("No budgets found.");
                        }
                        else
                        {
                            foreach (var budget in budgets)
                            {
                                Console.WriteLine($"{budget.Id} | {budget.Year}-{budget.Month:00} | {budget.Category,-15} | {budget.LimitAmount,10:C}");
                            }
                        }

                        ConsoleHelper.Pause();
                        break;
                    case 3:
                        var items = await _client.GetBudgetsAsync(user.Id);
                        if (items.Count == 0)
                        {
                            ConsoleHelper.WriteInfo("No budgets available to delete.");
                            ConsoleHelper.Pause();
                            break;
                        }

                        ConsoleHelper.WriteIndexedList(items.Select(b => $"{b.Year}-{b.Month:00} | {b.Category,-15} | {b.LimitAmount,10:C}"));
                        var budgetSelection = ConsoleHelper.PromptInt("Select budget number", 1, items.Count);
                        await _client.DeleteBudgetAsync(new DeleteBudgetRequest
                        {
                            UserId = user.Id,
                            BudgetId = items[budgetSelection - 1].Id
                        });
                        ConsoleHelper.WriteSuccess("Budget deleted.");
                        ConsoleHelper.Pause();
                        break;
                    case 4:
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
}
