using Domain.Entities;
using FinanceTracker.Contracts.Transactions;

namespace App.Services.Interfaces;

public interface ITransactionService
{
    Task<string?> AddAsync(CreateTransactionRequest request);
    Task<IReadOnlyList<Transaction>> GetByUserAsync(Guid userId);
    Task DeleteAsync(DeleteTransactionRequest request);
}
