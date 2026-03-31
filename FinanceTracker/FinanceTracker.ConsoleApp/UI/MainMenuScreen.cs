using FinanceTracker.Contracts.Auth;
using Shared.Helpers;

namespace FinanceTracker.ConsoleApp.UI;

public class MainMenuScreen
{
    private readonly TransactionScreen _transactionScreen;
    private readonly BudgetScreen _budgetScreen;
    private readonly ReportScreen _reportScreen;

    public MainMenuScreen(ApiClient client)
    {
        _transactionScreen = new TransactionScreen(client);
        _budgetScreen = new BudgetScreen(client);
        _reportScreen = new ReportScreen(client);
    }

    public async Task<bool> ShowAsync(UserResponse user, string apiBaseUrl)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader($"Main Menu - {user.UserName}");
            Console.WriteLine($"API: {apiBaseUrl}");
            Console.WriteLine();

            var choice = ConsoleHelper.PromptMenu("Choose an option:", "Transactions", "Budgets", "Reports", "Logout", "Exit");

            switch (choice)
            {
                case 1:
                    await _transactionScreen.ShowAsync(user);
                    break;
                case 2:
                    await _budgetScreen.ShowAsync(user);
                    break;
                case 3:
                    await _reportScreen.ShowAsync(user);
                    break;
                case 4:
                    ConsoleHelper.WriteInfo("Logged out.");
                    ConsoleHelper.Pause();
                    return false;
                case 5:
                    ConsoleHelper.WriteInfo("Goodbye.");
                    return true;
            }
        }
    }
}
