namespace Domain.Entities;

public class MonthlyReport
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public int TransactionCount { get; set; }
    public decimal Savings => TotalIncome - TotalExpense;
}
