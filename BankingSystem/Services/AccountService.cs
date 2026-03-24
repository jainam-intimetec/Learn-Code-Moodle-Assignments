using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.Services;

public class AccountService : IAccountService
{
    private readonly IFileStorageService _storage;

    public AccountService(IFileStorageService storage)
    {
        _storage = storage;
    }

    public void Deposit(User user, decimal amount)
    {
        ValidateDepositAmount(amount);
        var originalBalance = user.Balance;

        try
        {
            IncreaseBalance(user, amount);
            Save(user);
        }
        catch
        {
            user.Balance = originalBalance;
            throw;
        }
    }

    public void Withdraw(User user, decimal amount)
    {
        ValidateWithdrawal(user, amount);
        var originalBalance = user.Balance;

        try
        {
            DecreaseBalance(user, amount);
            Save(user);
        }
        catch
        {
            user.Balance = originalBalance;
            throw;
        }
    }


    private void ValidateDepositAmount(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Invalid deposit amount.");
    }

    private void ValidateWithdrawal(User user, decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Invalid withdrawal amount.");

        if (amount > user.Balance)
            throw new InvalidOperationException("Insufficient balance.");
    }

    private void IncreaseBalance(User user, decimal amount)
    {
        user.Balance += amount;
    }

    private void DecreaseBalance(User user, decimal amount)
    {
        user.Balance -= amount;
    }

    private void Save(User user)
    {
        var users = _storage.LoadUsers();
        var index = users.FindIndex(u => u.AccountId == user.AccountId);

        if (index < 0)
            throw new InvalidOperationException("Account could not be found.");

        users[index] = user;
        _storage.SaveUsers(users);
    }
}
