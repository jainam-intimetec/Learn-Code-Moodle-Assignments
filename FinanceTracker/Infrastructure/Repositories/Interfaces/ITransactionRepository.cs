using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId);
    Task<Transaction?> GetByIdAsync(Guid transactionId);
    Task DeleteAsync(Guid transactionId);
}
