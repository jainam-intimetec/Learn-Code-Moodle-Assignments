using FinanceTracker.Contracts.Auth;
using FinanceTracker.Contracts.Reports;
using FinanceTracker.Contracts.Transactions;
using FinanceTracker.Domain.Enums;
using Shared.Helpers;

namespace FinanceTracker.ConsoleApp.UI;

public class TransactionScreen
{
    private readonly ApiClient _client;

    public TransactionScreen(ApiClient client)
    {
        _client = client;
    }

    public async Task ShowAsync(UserResponse user)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("Transactions");
            var choice = ConsoleHelper.PromptMenu("Choose an option:", "Add income", "Add expense", "View all", "Delete", "Back");

            try
            {
                switch (choice)
                {
                    case 1:
                    case 2:
                        var result = await _client.AddTransactionAsync(new CreateTransactionRequest
                        {
                            UserId = user.Id,
                            Type = choice == 1 ? TransactionType.Income : TransactionType.Expense,
                            Amount = ConsoleHelper.PromptDecimal("Amount", 0.01m, 100000000m),
                            Category = ConsoleHelper.PromptRequiredText("Category", 2, 50),
                            Note = ConsoleHelper.PromptOptionalText("Note", 100),
                            Date = ConsoleHelper.PromptDate("Transaction date", DateTime.Today)
                        });

                        ConsoleHelper.WriteSuccess(result.Message);
                        if (!string.IsNullOrWhiteSpace(result.BudgetAlertMessage))
                            ConsoleHelper.WriteError(result.BudgetAlertMessage);

                        ConsoleHelper.Pause();
                        break;
                    case 3:
                        var transactions = await _client.GetTransactionsAsync(user.Id);
                        if (transactions.Count == 0)
                        {
                            ConsoleHelper.WriteInfo("No transactions found.");
                        }
                        else
                        {
                            foreach (var transaction in transactions)
                            {
                                Console.WriteLine($"{transaction.Id} | {transaction.DateUtc:yyyy-MM-dd} | {transaction.Type,-7} | {transaction.Amount,10:C} | {transaction.Category} | {transaction.Note}");
                            }
                        }

                        ConsoleHelper.Pause();
                        break;
                    case 4:
                        var items = await _client.GetTransactionsAsync(user.Id);
                        if (items.Count == 0)
                        {
                            ConsoleHelper.WriteInfo("No transactions available to delete.");
                            ConsoleHelper.Pause();
                            break;
                        }

                        ConsoleHelper.WriteIndexedList(items.Select(t => $"{t.DateUtc:yyyy-MM-dd} | {t.Type,-7} | {t.Amount,10:C} | {t.Category}"));
                        var transactionSelection = ConsoleHelper.PromptInt("Select transaction number", 1, items.Count);
                        await _client.DeleteTransactionAsync(new DeleteTransactionRequest
                        {
                            UserId = user.Id,
                            TransactionId = items[transactionSelection - 1].Id
                        });
                        ConsoleHelper.WriteSuccess("Transaction deleted.");
                        ConsoleHelper.Pause();
                        break;
                    case 5:
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
