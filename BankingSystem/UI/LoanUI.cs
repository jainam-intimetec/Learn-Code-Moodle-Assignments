using BankingSystem.DependencyDtos;
using BankingSystem.Exceptions;
using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.UI;

public class LoanUI
{
    private readonly ILoanService _loanService;
    private readonly UiCommonServicesDto _ui;

    public LoanUI(LoanUiDependenciesDto dependencies)
    {
        _loanService = dependencies.LoanService;
        _ui = dependencies.Ui;
    }

    public void Show(User user)
    {
        while (true)
        {
            try
            {
                DisplayMenu();
                var choice = ReadMenuChoice();

                if (choice == "6")
                    return;

                HandleChoice(choice, user);
            }
            catch (BankingException ex)
            {
                _ui.Console.ShowError(ex.Message);
            }
            catch (Exception)
            {
                _ui.Console.ShowError("Unexpected error while processing loan operations.");
            }

            _ui.Console.Pause();
        }
    }

    private void DisplayMenu()
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
        return _ui.InputValidation.RequireString(Console.ReadLine(), "Menu option");
    }

    private void HandleChoice(string choice, User user)
    {
        switch (choice)
        {
            case "1": ShowSummary(user); break;
            case "2": ApplyLoan(user); break;
            case "3": PayEmi(user); break;
            case "4": CalculateEmi(); break;
            case "5": SettleLoan(user); break;
            default:
                throw new BankingException("Invalid menu option selected.");
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
        var principal = ReadDecimal("Enter principal amount: ", "Principal Amount");
        var tenure = ReadDecimal("Enter tenure (months): ", "Tenure");

        var loan = _loanService.CalculateLoan(principal, (int)tenure);
        _loanService.ApplyLoan(user, loan);

        Console.WriteLine($"Loan applied successfully. Monthly EMI: {loan.MonthlyEmi} Rs");
    }

    private void PayEmi(User user)
    {
        EnsureLoanExists(user);

        Console.WriteLine($"EMI to pay this month: {user.Loan!.MonthlyEmi} Rs");
        _loanService.PayMonthlyEmi(user);

        Console.WriteLine($"Remaining Loan Amount: {user.Loan.RemainingAmount} Rs");
    }

    private void SettleLoan(User user)
    {
        EnsureLoanExists(user);

        Console.WriteLine($"Remaining Loan Amount: {user.Loan!.RemainingAmount} Rs");
        Console.Write("Do you want to settle the loan? (y/n): ");

        if (!IsConfirmed())
            return;

        _loanService.SettleLoan(user);
        Console.WriteLine($"Loan settled successfully. Balance: {user.Balance} Rs");
    }

    private void CalculateEmi()
    {
        var principal = ReadDecimal("Enter principal: ", "Principal");
        var tenure = ReadDecimal("Enter tenure (months): ", "Tenure");

        var loan = _loanService.CalculateLoan(principal, (int)tenure);

        Console.WriteLine($"Interest Rate : {loan.InterestRate} %");
        Console.WriteLine($"Monthly EMI   : {loan.MonthlyEmi} Rs");
        Console.WriteLine($"Total Payable : {loan.MonthlyEmi * tenure} Rs");
    }

    private decimal ReadDecimal(string prompt, string field)
    {
        Console.Write(prompt);
        return _ui.InputValidation.RequireDecimal(Console.ReadLine(), field);
    }

    private static bool IsConfirmed()
    {
        return Console.ReadLine()?.Trim().ToLower() == "y";
    }

    private static void EnsureLoanExists(User user)
    {
        if (user.Loan == null)
            throw new BankingException("No active loan found.");
    }
}
