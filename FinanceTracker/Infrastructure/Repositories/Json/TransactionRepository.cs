using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Shared.Helpers;

namespace Infrastructure.Repositories.Json;

public class TransactionRepository : ITransactionRepository
{
    private readonly string _path = StoragePathHelper.GetDataFilePath("transactions.json");

    public async Task AddAsync(Transaction transaction)
    {
        var transactions = await JsonStorageHelper.ReadAsync<Transaction>(_path);
        transactions.Add(transaction);
        await JsonStorageHelper.WriteAsync(_path, transactions);
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId)
    {
        var transactions = await JsonStorageHelper.ReadAsync<Transaction>(_path);

        return transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.DateUtc)
            .ThenByDescending(t => t.Id)
            .ToList();
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId)
    {
        var transactions = await JsonStorageHelper.ReadAsync<Transaction>(_path);
        return transactions.FirstOrDefault(t => t.Id == transactionId);
    }

    public async Task DeleteAsync(Guid transactionId)
    {
        var transactions = await JsonStorageHelper.ReadAsync<Transaction>(_path);
        var removedCount = transactions.RemoveAll(t => t.Id == transactionId);

        if (removedCount > 0)
            await JsonStorageHelper.WriteAsync(_path, transactions);
    }
}
