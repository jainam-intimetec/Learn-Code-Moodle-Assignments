using BankingSystem.DependencyDtos;
using BankingSystem.Models;

namespace BankingSystem.UI;

public class DashboardUI
{
    private readonly DashboardUiDependenciesDto _deps;

    public DashboardUI(DashboardUiDependenciesDto dependencies)
    {
        _deps = dependencies;
    }

    public void Show(User user)
    {
        while (true)
        {
            try
            {
                Console.Clear();
                DisplayHeader(user);
                DisplayMenu();

                var choice = ReadMenuChoice();

                if (choice == "6")
                    return;

                HandleChoice(choice, user);
            }
            catch (Exception ex)
            {
                _deps.Ui.Console.ShowError(ex.Message);
            }

            _deps.Ui.Console.Pause();
        }
    }

    private void DisplayHeader(User user)
    {
        Console.WriteLine($"Welcome {user.FirstName} {user.LastName}");
        Console.WriteLine($"Account ID: {user.AccountId}");
        Console.WriteLine("----------------------------------");
    }

    private void DisplayMenu()
    {
        Console.WriteLine("1. Check Balance");
        Console.WriteLine("2. Deposit");
        Console.WriteLine("3. Withdraw");
        Console.WriteLine("4. Transfer");
        Console.WriteLine("5. Loan Management");
        Console.WriteLine("6. Logout");
        Console.Write("Select option: ");
    }

    private string ReadMenuChoice()
    {
        return _deps.Ui.InputValidation.RequireString(
            Console.ReadLine(), "Menu option");
    }

    private void HandleChoice(string choice, User user)
    {
        switch (choice)
        {
            case "1": ShowBalance(user); break;
            case "2": Deposit(user); break;
            case "3": Withdraw(user); break;
            case "4": Transfer(user); break;
            case "5": _deps.LoanUI.Show(user); break;
            default:
                throw new InvalidOperationException("Invalid menu option selected.");
        }
    }

    private void ShowBalance(User user)
    {
        Console.WriteLine($"Current Balance: {user.Balance} Rs");
    }

    private void Deposit(User user)
    {
        Console.Write("Enter deposit amount: ");
        var amount = _deps.Ui.InputValidation.RequireDecimal(
            Console.ReadLine(), "Deposit Amount");

        _deps.AccountService.Deposit(user, amount);
        Console.WriteLine($"Updated Balance: {user.Balance} Rs");
    }

    private void Withdraw(User user)
    {
        Console.Write("Enter withdrawal amount: ");
        var amount = _deps.Ui.InputValidation.RequireDecimal(
            Console.ReadLine(), "Withdrawal Amount");

        _deps.AccountService.Withdraw(user, amount);
        Console.WriteLine($"Remaining Balance: {user.Balance} Rs");
    }

    private void Transfer(User user)
    {
        Console.Write("Enter target Account ID: ");
        var targetAccountId = _deps.Ui.InputValidation.RequireString(
            Console.ReadLine(), "Target Account ID");

        Console.Write("Enter transfer amount: ");
        var amount = _deps.Ui.InputValidation.RequireDecimal(
            Console.ReadLine(), "Transfer Amount");

        var request = new TransferRequest
        {
            Sender = user,
            TargetAccountId = targetAccountId,
            Amount = amount
        };

        _deps.TransactionService.Transfer(request);

        Console.WriteLine($"Remaining Balance: {user.Balance} Rs");
    }
}
