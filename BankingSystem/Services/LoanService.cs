using BankingSystem.Enums;
using BankingSystem.Exceptions;
using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.Services;

public class LoanService : ILoanService
{
    private readonly IFileStorageService _storage;
    private readonly IInterestRateProvider _interestRateProvider;

    public LoanService(
        IFileStorageService storage,
        IInterestRateProvider interestRateProvider)
    {
        _storage = storage;
        _interestRateProvider = interestRateProvider;
    }

    public Loan CalculateLoan(decimal principal, int tenureMonths)
    {
        ValidateLoanInputs(principal, tenureMonths);
        var interestRate = _interestRateProvider.GetInterestRate(principal, tenureMonths);

        var totalPayable = principal * (decimal)(1 + interestRate / 100);
        var monthlyEmi = totalPayable / tenureMonths;

        return new Loan
        {
            Principal = principal,
            InterestRate = (decimal)interestRate,
            TenureMonths = tenureMonths,
            MonthlyEmi = monthlyEmi,
            RemainingAmount = totalPayable,
            EmiPaidCount = 0,
            Status = LoanStatus.Active
        };
    }

    public void ApplyLoan(User user, Loan loan)
    {
        EnsureNoActiveLoan(user);
        var originalBalance = user.Balance;
        var originalLoan = user.Loan;

        try
        {
            user.Loan = loan;
            user.Balance += loan.Principal;
            Save(user);
        }
        catch
        {
            user.Balance = originalBalance;
            user.Loan = originalLoan;
            throw;
        }
    }

    public void PayMonthlyEmi(User user)
    {
        EnsureLoanExists(user);
        ProcessLoanPayment(user, user.Loan!.MonthlyEmi);
    }

    public void SettleLoan(User user)
    {
        EnsureLoanExists(user);
        ProcessLoanPayment(user, user.Loan!.RemainingAmount);
    }

    private void ProcessLoanPayment(User user, decimal paymentAmount)
    {
        EnsureActiveLoan(user);
        EnsureSufficientBalance(user, paymentAmount);
        var originalBalance = user.Balance;
        var originalRemainingAmount = user.Loan!.RemainingAmount;
        var originalEmiPaidCount = user.Loan.EmiPaidCount;
        var originalStatus = user.Loan.Status;

        try
        {
            DeductAmount(user, paymentAmount);
            CloseLoanIfCompleted(user);
            Save(user);
        }
        catch
        {
            user.Balance = originalBalance;
            user.Loan!.RemainingAmount = originalRemainingAmount;
            user.Loan.EmiPaidCount = originalEmiPaidCount;
            user.Loan.Status = originalStatus;
            throw;
        }
    }

    private static void ValidateLoanInputs(decimal principal, int tenureMonths)
    {
        if (principal <= 0)
            throw new BankingException("Principal amount must be greater than zero.");

        if (tenureMonths <= 0)
            throw new BankingException("Tenure must be greater than zero.");
    }

    private static void EnsureNoActiveLoan(User user)
    {
        if (user.Loan is { Status: LoanStatus.Active })
            throw new BankingException("Only one active loan allowed.");
    }

    private static void EnsureLoanExists(User user)
    {
        if (user.Loan == null)
            throw new BankingException("No loan found.");
    }

    private static void EnsureActiveLoan(User user)
    {
        if (user.Loan == null || user.Loan.Status == LoanStatus.Closed)
            throw new BankingException("No active loan found.");
    }

    private static void EnsureSufficientBalance(User user, decimal amount)
    {
        if (user.Balance < amount)
            throw new BankingException("Insufficient balance.");
    }

    private static void DeductAmount(User user, decimal amount)
    {
        user.Balance -= amount;
        user.Loan!.RemainingAmount -= amount;
        user.Loan.EmiPaidCount++;
    }

    private static void CloseLoanIfCompleted(User user)
    {
        if (user.Loan!.RemainingAmount <= 0)
        {
            user.Loan.RemainingAmount = 0;
            user.Loan.Status = LoanStatus.Closed;
        }
    }

    private void Save(User user)
    {
        var users = _storage.LoadUsers();
        var index = users.FindIndex(u => u.AccountId == user.AccountId);

        if (index < 0)
            throw new BankingException("Account could not be found.");

        users[index] = user;
        _storage.SaveUsers(users);
    }
}
