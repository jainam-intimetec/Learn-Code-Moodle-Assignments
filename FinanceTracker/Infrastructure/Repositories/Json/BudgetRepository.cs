using Domain.Entities;
using FinanceTracker.Contracts.Budgets;
using Infrastructure.Repositories.Interfaces;
using Shared.Helpers;

namespace Infrastructure.Repositories.Json;

public class BudgetRepository : IBudgetRepository
{
    private readonly string _path = StoragePathHelper.GetDataFilePath("budgets.json");

    public async Task AddOrUpdateAsync(Budget budget)
    {
        var budgets = await JsonStorageHelper.ReadAsync<Budget>(_path);
        var existingIndex = budgets.FindIndex(b => b.Id == budget.Id);

        if (existingIndex >= 0)
            budgets[existingIndex] = budget;
        else
            budgets.Add(budget);

        await JsonStorageHelper.WriteAsync(_path, budgets);
    }

    public async Task<IReadOnlyList<Budget>> GetByUserIdAsync(Guid userId)
    {
        var budgets = await JsonStorageHelper.ReadAsync<Budget>(_path);

        return budgets
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.Year)
            .ThenBy(b => b.Month)
            .ThenBy(b => b.Category)
            .ToList();
    }

    public async Task<Budget?> GetByIdAsync(Guid budgetId)
    {
        var budgets = await JsonStorageHelper.ReadAsync<Budget>(_path);
        return budgets.FirstOrDefault(b => b.Id == budgetId);
    }

    public async Task<Budget?> GetByCategoryAsync(BudgetLookupRequest request)
    {
        var budgets = await JsonStorageHelper.ReadAsync<Budget>(_path);

        return budgets.FirstOrDefault(b =>
            b.UserId == request.UserId &&
            b.Year == request.Year &&
            b.Month == request.Month &&
            string.Equals(b.Category, request.Category, StringComparison.OrdinalIgnoreCase));
    }

    public async Task DeleteAsync(Guid budgetId)
    {
        var budgets = await JsonStorageHelper.ReadAsync<Budget>(_path);
        var removedCount = budgets.RemoveAll(b => b.Id == budgetId);

        if (removedCount > 0)
            await JsonStorageHelper.WriteAsync(_path, budgets);
    }
}
