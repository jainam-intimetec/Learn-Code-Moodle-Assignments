using Domain.Entities;
using FinanceTracker.Contracts.Budgets;

namespace Infrastructure.Repositories.Interfaces;

public interface IBudgetRepository
{
    Task AddOrUpdateAsync(Budget budget);
    Task<IReadOnlyList<Budget>> GetByUserIdAsync(Guid userId);
    Task<Budget?> GetByIdAsync(Guid budgetId);
    Task<Budget?> GetByCategoryAsync(BudgetLookupRequest request);
    Task DeleteAsync(Guid budgetId);
}
