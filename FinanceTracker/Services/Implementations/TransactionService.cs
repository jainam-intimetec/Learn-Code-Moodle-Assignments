using App.Services.Interfaces;
using Domain.Entities;
using FinanceTracker.Contracts.Transactions;
using FinanceTracker.Domain.Enums;
using Infrastructure.Repositories.Interfaces;
using Shared.Exceptions;

namespace App.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetService _budgetService;

    public TransactionService(ITransactionRepository transactionRepository, IBudgetService budgetService)
    {
        _transactionRepository = transactionRepository;
        _budgetService = budgetService;
    }

    public async Task<string?> AddAsync(CreateTransactionRequest request)
    {
        var transaction = Transaction.Create(request);
        await _transactionRepository.AddAsync(transaction);

        if (request.Type != TransactionType.Expense)
            return null;

        var currentMonthTransactions = await _transactionRepository.GetByUserIdAsync(request.UserId);
        var currentMonthSpent = currentMonthTransactions
            .Where(t =>
                t.Type == TransactionType.Expense &&
                t.DateUtc.Year == transaction.DateUtc.Year &&
                t.DateUtc.Month == transaction.DateUtc.Month &&
                string.Equals(t.Category, transaction.Category, StringComparison.OrdinalIgnoreCase))
            .Sum(t => t.Amount);

        return await _budgetService.GetBudgetExceededMessageAsync(transaction, currentMonthSpent);
    }

    public Task<IReadOnlyList<Transaction>> GetByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("User is required.");

        return _transactionRepository.GetByUserIdAsync(userId);
    }

    public async Task DeleteAsync(DeleteTransactionRequest request)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
        if (transaction is null || transaction.UserId != request.UserId)
            throw new NotFoundException("Transaction not found.");

        await _transactionRepository.DeleteAsync(request.TransactionId);
    }
}
