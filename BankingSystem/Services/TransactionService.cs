using BankingSystem.Exceptions;
using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.Services;

public class TransactionService : ITransactionService
{
    private readonly IFileStorageService _storage;

    public TransactionService(IFileStorageService storage)
    {
        _storage = storage;
    }

    public void Transfer(TransferRequest request)
    {
        Validate(request);

        var users = _storage.LoadUsers();
        var sender = FindUser(users, request.Sender.AccountId, "Sender account not found.");
        var receiver = FindReceiver(users, request.TargetAccountId);
        var originalSenderBalance = sender.Balance;
        var originalReceiverBalance = receiver.Balance;

        try
        {
            ExecuteTransfer(sender, receiver, request.Amount);
            _storage.SaveUsers(users);
            request.Sender.Balance = sender.Balance;
        }
        catch
        {
            sender.Balance = originalSenderBalance;
            receiver.Balance = originalReceiverBalance;
            request.Sender.Balance = originalSenderBalance;
            throw;
        }
    }

    private static void Validate(TransferRequest request)
    {
        if (request.Amount <= 0)
            throw new BankingException("Transfer amount must be greater than zero.");

        if (request.Sender.Balance < request.Amount)
            throw new BankingException("Insufficient balance.");

        if (request.Sender.AccountId == request.TargetAccountId)
            throw new BankingException("Cannot transfer to the same account.");
    }

    private static User FindUser(IEnumerable<User> users, string accountId, string errorMessage)
    {
        var user = users.FirstOrDefault(u => u.AccountId == accountId);

        if (user == null)
            throw new BankingException(errorMessage);

        return user;
    }

    private static User FindReceiver(IEnumerable<User> users, string targetAccountId)
    {
        return FindUser(users, targetAccountId, "Target account not found.");
    }

    private static void ExecuteTransfer(User sender, User receiver, decimal amount)
    {
        sender.Balance -= amount;
        receiver.Balance += amount;
    }
}
