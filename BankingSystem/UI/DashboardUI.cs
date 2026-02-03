using BankingSystem.Interfaces;
using BankingSystem.Models;
using BankingSystem.Services;

namespace BankingSystem.UI;

public class DashboardUI
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly LoanUI _loanUI;
    private readonly InputValidationService _inputValidationService;
    private readonly ConsoleService _consoleService;

    public DashboardUI(
        IAccountService accountService,
        ITransactionService transactionService,
        LoanUI loanUI,
        InputValidationService inputValidationService,
        ConsoleService consoleService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _loanUI = loanUI;
        _inputValidationService = inputValidationService;
        _consoleService = consoleService;
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

                HandleMenuChoice(choice, user);
            }
            catch (Exception ex)
            {
                _consoleService.ShowError(ex.Message);
            }

            _consoleService.Pause();
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
        return _inputValidationService.RequireString(
            Console.ReadLine(),
            "Menu option");
    }

    private void HandleMenuChoice(string choice, User user)
    {
        switch (choice)
        {
            case "1":
                ShowBalance(user);
                break;
            case "2":
                HandleDeposit(user);
                break;
            case "3":
                HandleWithdraw(user);
                break;
            case "4":
                HandleTransfer(user);
                break;
            case "5":
                _loanUI.Show(user);
                break;
            default:
                throw new Exception("Invalid menu option selected.");
        }
    }


    private void ShowBalance(User user)
    {
        Console.WriteLine($"Current Balance: {user.Balance} Rs");
    }

    private void HandleDeposit(User user)
    {
        Console.Write("Enter deposit amount: ");
        var amount = _inputValidationService.RequireDecimal(
            Console.ReadLine(),
            "Deposit Amount");

        _accountService.Deposit(user, amount);

        Console.WriteLine("Deposit successful.");
        Console.WriteLine($"Updated Balance: {user.Balance} Rs");
    }

    private void HandleWithdraw(User user)
    {
        Console.Write("Enter withdrawal amount: ");
        var amount = _inputValidationService.RequireDecimal(
            Console.ReadLine(),
            "Withdrawal Amount");

        _accountService.Withdraw(user, amount);

        Console.WriteLine("Withdrawal successful.");
        Console.WriteLine($"Remaining Balance: {user.Balance} Rs");
    }

    private void HandleTransfer(User user)
    {
        Console.Write("Enter target Account ID: ");
        var targetAccountId = _inputValidationService.RequireString(
            Console.ReadLine(),
            "Target Account ID");

        Console.Write("Enter transfer amount: ");
        var amount = _inputValidationService.RequireDecimal(
            Console.ReadLine(),
            "Transfer Amount");

        _transactionService.Transfer(user, targetAccountId, amount);

        Console.WriteLine("Transfer completed successfully.");
        Console.WriteLine($"Remaining Balance: {user.Balance} Rs");
    }
}
