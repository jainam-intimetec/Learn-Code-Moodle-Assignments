using FinanceTracker.Contracts.Transactions;
using FinanceTracker.Domain.Enums;
using Shared.Exceptions;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime DateUtc { get; set; }

    public static Transaction Create(CreateTransactionRequest request)
    {
        Validate(request);

        return new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Type = request.Type,
            Amount = decimal.Round(request.Amount, 2, MidpointRounding.AwayFromZero),
            Category = request.Category.Trim(),
            Note = string.IsNullOrWhiteSpace(request.Note) ? "-" : request.Note.Trim(),
            DateUtc = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc)
        };
    }

    public static void Validate(CreateTransactionRequest request)
    {
        if (request.UserId == Guid.Empty)
            throw new ValidationException("User is required.");

        if (request.Amount <= 0)
            throw new ValidationException("Amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(request.Category) || request.Category.Trim().Length < 2)
            throw new ValidationException("Category must be at least 2 characters long.");

        if (!string.IsNullOrWhiteSpace(request.Note) && request.Note.Trim().Length > 100)
            throw new ValidationException("Note cannot exceed 100 characters.");

        if (request.Date.Date > DateTime.Today)
            throw new ValidationException("Transaction date cannot be in the future.");
    }
}
