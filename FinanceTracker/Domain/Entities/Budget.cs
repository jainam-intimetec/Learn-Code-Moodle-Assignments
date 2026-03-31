using FinanceTracker.Contracts.Budgets;
using Shared.Exceptions;

namespace Domain.Entities;

public class Budget
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal LimitAmount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public static Budget Create(UpsertBudgetRequest request)
    {
        Validate(request);

        return new Budget
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Category = request.Category.Trim(),
            LimitAmount = decimal.Round(request.LimitAmount, 2, MidpointRounding.AwayFromZero),
            Year = request.Year,
            Month = request.Month,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static void Validate(UpsertBudgetRequest request)
    {
        if (request.UserId == Guid.Empty)
            throw new ValidationException("User is required.");

        if (string.IsNullOrWhiteSpace(request.Category) || request.Category.Trim().Length < 2)
            throw new ValidationException("Category must be at least 2 characters long.");

        if (request.LimitAmount <= 0)
            throw new ValidationException("Budget limit must be greater than zero.");

        if (request.Month < 1 || request.Month > 12)
            throw new ValidationException("Month must be between 1 and 12.");

        if (request.Year < DateTime.UtcNow.Year - 5 || request.Year > DateTime.UtcNow.Year + 5)
            throw new ValidationException("Year is out of allowed range.");
    }
}
