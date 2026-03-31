using App.Services.Interfaces;
using Domain.Entities;
using FinanceTracker.Contracts.Budgets;
using Infrastructure.Adapters.Interfaces;
using Infrastructure.Adapters.Models;
using Infrastructure.Repositories.Interfaces;
using Shared.Exceptions;

namespace App.Services.Implementations;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly INotificationService _notificationService;

    public BudgetService(IBudgetRepository budgetRepository, INotificationService notificationService)
    {
        _budgetRepository = budgetRepository;
        _notificationService = notificationService;
    }

    public async Task AddOrUpdateAsync(UpsertBudgetRequest request)
    {
        request.Category = request.Category.Trim();
        Budget.Validate(request);

        var existingBudget = await _budgetRepository.GetByCategoryAsync(new BudgetLookupRequest
        {
            UserId = request.UserId,
            Category = request.Category,
            Year = request.Year,
            Month = request.Month
        });

        if (existingBudget is null)
        {
            await _budgetRepository.AddOrUpdateAsync(Budget.Create(request));
            return;
        }

        existingBudget.Category = request.Category;
        existingBudget.LimitAmount = decimal.Round(request.LimitAmount, 2, MidpointRounding.AwayFromZero);
        existingBudget.Year = request.Year;
        existingBudget.Month = request.Month;

        await _budgetRepository.AddOrUpdateAsync(existingBudget);
    }

    public Task<IReadOnlyList<Budget>> GetByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("User is required.");

        return _budgetRepository.GetByUserIdAsync(userId);
    }

    public async Task DeleteAsync(DeleteBudgetRequest request)
    {
        var budget = await _budgetRepository.GetByIdAsync(request.BudgetId);
        if (budget is null || budget.UserId != request.UserId)
            throw new NotFoundException("Budget not found.");

        await _budgetRepository.DeleteAsync(request.BudgetId);
    }

    public async Task<string?> GetBudgetExceededMessageAsync(Transaction transaction, decimal currentMonthSpent)
    {
        var budget = await _budgetRepository.GetByCategoryAsync(new BudgetLookupRequest
        {
            UserId = transaction.UserId,
            Category = transaction.Category,
            Year = transaction.DateUtc.Year,
            Month = transaction.DateUtc.Month
        });

        if (budget is null || currentMonthSpent <= budget.LimitAmount)
            return null;

        return await _notificationService.SendBudgetExceededAlertAsync(new BudgetExceededAlert
        {
            Category = transaction.Category,
            Year = transaction.DateUtc.Year,
            Month = transaction.DateUtc.Month,
            LimitAmount = budget.LimitAmount,
            SpentAmount = currentMonthSpent
        });
    }
}
