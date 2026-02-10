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
        var receiver = FindReceiver(users, request.TargetAccountId);

        ExecuteTransfer(request.Sender, receiver, request.Amount);

        _storage.SaveUsers(users);
    }
    private static void Validate(TransferRequest request)
    {
        if (request.Amount <= 0)
            throw new InvalidOperationException("Transfer amount must be greater than zero.");

        if (request.Sender.Balance < request.Amount)
            throw new InvalidOperationException("Insufficient balance.");
    }
    private static User FindReceiver(IEnumerable<User> users, string targetAccountId)
    {
        var receiver = users.FirstOrDefault(u => u.AccountId == targetAccountId);

        if (receiver == null)
            throw new InvalidOperationException("Target account not found.");

        return receiver;
    }

    private static void ExecuteTransfer(User sender, User receiver, decimal amount)
    {
        sender.Balance -= amount;
        receiver.Balance += amount;
    }
}
