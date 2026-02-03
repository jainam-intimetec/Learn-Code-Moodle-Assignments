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

    public void Transfer(User sender, string targetAccountId, decimal amount)
    {
        if (amount <= 0 || sender.Balance < amount)
            throw new Exception("Invalid transfer.");

        var users = _storage.LoadUsers();
        var receiver = users.FirstOrDefault(u => u.AccountId == targetAccountId);

        if (receiver == null)
            throw new Exception("Target account not found.");

        sender.Balance -= amount;
        receiver.Balance += amount;

        _storage.SaveUsers(users);
    }
}
