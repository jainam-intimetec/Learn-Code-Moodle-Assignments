namespace Infrastructure.Adapters.Models;

public class BudgetExceededAlert
{
    public string Category { get; init; } = string.Empty;
    public int Year { get; init; }
    public int Month { get; init; }
    public decimal LimitAmount { get; init; }
    public decimal SpentAmount { get; init; }
}
