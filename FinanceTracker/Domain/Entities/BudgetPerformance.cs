namespace Domain.Entities;

public class BudgetPerformance
{
    public string Category { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount => BudgetAmount - SpentAmount;
    public bool IsOverBudget => SpentAmount > BudgetAmount;
}
