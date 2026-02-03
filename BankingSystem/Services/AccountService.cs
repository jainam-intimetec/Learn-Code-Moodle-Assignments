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
        IncreaseBalance(user, amount);
        Save(user);
    }

    public void Withdraw(User user, decimal amount)
    {
        ValidateWithdrawal(user, amount);
        DecreaseBalance(user, amount);
        Save(user);
    }


    private void ValidateDepositAmount(decimal amount)
    {
        if (amount <= 0)
            throw new Exception("Invalid deposit amount.");
    }

    private void ValidateWithdrawal(User user, decimal amount)
    {
        if (amount <= 0)
            throw new Exception("Invalid withdrawal amount.");

        if (amount > user.Balance)
            throw new Exception("Insufficient balance.");
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
        users[users.FindIndex(u => u.AccountId == user.AccountId)] = user;
        _storage.SaveUsers(users);
    }
}
