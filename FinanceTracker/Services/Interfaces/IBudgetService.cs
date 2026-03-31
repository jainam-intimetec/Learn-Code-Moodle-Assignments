using Domain.Entities;
using FinanceTracker.Contracts.Budgets;

namespace App.Services.Interfaces;

public interface IBudgetService
{
    Task AddOrUpdateAsync(UpsertBudgetRequest request);
    Task<IReadOnlyList<Budget>> GetByUserAsync(Guid userId);
    Task DeleteAsync(DeleteBudgetRequest request);
    Task<string?> GetBudgetExceededMessageAsync(Transaction transaction, decimal currentMonthSpent);
}
