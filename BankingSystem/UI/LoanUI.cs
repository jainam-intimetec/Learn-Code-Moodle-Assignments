using BankingSystem.Interfaces;
using BankingSystem.Models;
using BankingSystem.Services;

namespace BankingSystem.UI;

public class LoanUI
{
    private readonly ILoanService _loanService;
    private readonly InputValidationService _inputValidationService;
    private readonly ConsoleService _consoleService;

    public LoanUI(
        ILoanService loanService,
        InputValidationService inputValidationService,
        ConsoleService consoleService)
    {
        _loanService = loanService;
        _inputValidationService = inputValidationService;
        _consoleService = consoleService;
    }

    public void Show(User user)
    {
        while (true)
        {
            try
            {
                ShowMenu();
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


    private void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("---- Loan Management ----");
        Console.WriteLine("1. My Loan Summary");
        Console.WriteLine("2. Apply for Loan");
        Console.WriteLine("3. Pay EMI (This Month)");
        Console.WriteLine("4. EMI Calculator");
        Console.WriteLine("5. Settle Full Loan");
        Console.WriteLine("6. Back");
    }

    private string ReadMenuChoice()
    {
        Console.Write("Select an option: ");
        return _inputValidationService.RequireString(
            Console.ReadLine(),
            "Menu option");
    }

    private void HandleMenuChoice(string choice, User user)
    {
        switch (choice)
        {
            case "1":
                ShowSummary(user);
                break;
            case "2":
                ApplyLoan(user);
                break;
            case "3":
                PayEmi(user);
                break;
            case "4":
                CalculateEmi();
                break;
            case "5":
                SettleLoan(user);
                break;
            default:
                throw new Exception("Invalid menu option selected.");
        }
    }


    private void ShowSummary(User user)
    {
        if (user.Loan == null)
        {
            Console.WriteLine("No loan found.");
            return;
        }

        Console.WriteLine($"Principal        : {user.Loan.Principal} Rs");
        Console.WriteLine($"Interest Rate    : {user.Loan.InterestRate} %");
        Console.WriteLine($"Monthly EMI      : {user.Loan.MonthlyEmi} Rs");
        Console.WriteLine($"EMIs Paid        : {user.Loan.EmiPaidCount}");
        Console.WriteLine($"Remaining Amount : {user.Loan.RemainingAmount} Rs");
        Console.WriteLine($"Status           : {user.Loan.Status}");
    }

    private void ApplyLoan(User user)
    {
        Console.Write("Enter principal amount: ");
        var principal = _inputValidationService.RequireDecimal(
            Console.ReadLine(),
            "Principal Amount");

        Console.Write("Enter tenure (months): ");
        var tenure = _inputValidationService.RequireDecimal(
            Console.ReadLine(),
            "Tenure");

        var loan = _loanService.CalculateLoan(principal, (int)tenure);
        _loanService.ApplyLoan(user, loan);

        Console.WriteLine("Loan applied successfully.");
        Console.WriteLine($"Monthly EMI: {loan.MonthlyEmi} Rs");
    }

    private void PayEmi(User user)
    {
        if (user.Loan == null)
        {
            Console.WriteLine("No active loan found.");
            return;
        }

        Console.WriteLine($"EMI to pay this month: {user.Loan.MonthlyEmi} Rs");

        _loanService.PayMonthlyEmi(user);

        Console.WriteLine("EMI paid successfully for this month.");
        Console.WriteLine($"Remaining Loan Amount: {user.Loan.RemainingAmount} Rs");
    }

    private void SettleLoan(User user)
    {
        if (user.Loan == null)
        {
            Console.WriteLine("No active loan found.");
            return;
        }

        Console.WriteLine($"Remaining Loan Amount: {user.Loan.RemainingAmount} Rs");
        Console.Write("Do you want to settle the loan? (y/n): ");

        var confirm = Console.ReadLine();

        if (confirm?.Trim().ToLower() != "y")
            return;

        _loanService.SettleLoan(user);

        Console.WriteLine("Loan settled successfully.");
        Console.WriteLine($"Updated Balance: {user.Balance} Rs");
    }

    private void CalculateEmi()
    {
        Console.Write("Enter principal: ");
        var principal = _inputValidationService.RequireDecimal(
            Console.ReadLine(),
            "Principal");

        Console.Write("Enter tenure (months): ");
        var tenure = _inputValidationService.RequireDecimal(
            Console.ReadLine(),
            "Tenure");

        var loan = _loanService.CalculateLoan(principal, (int)tenure);

        Console.WriteLine($"Interest Rate : {loan.InterestRate} %");
        Console.WriteLine($"Monthly EMI   : {loan.MonthlyEmi} Rs");
        Console.WriteLine($"Total Payable : {loan.MonthlyEmi * tenure} Rs");
    }
}
