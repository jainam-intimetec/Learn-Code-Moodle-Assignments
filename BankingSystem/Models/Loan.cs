namespace BankingSystem.Models;

public class Loan
{
    public decimal Principal { get; init; }
    public decimal InterestRate { get; init; }
    public int TenureMonths { get; init; }

    public decimal MonthlyEmi { get; init; }
    public decimal RemainingAmount { get; set; }
    public int EmiPaidCount { get; set; }

    public LoanStatus Status { get; set; }
}
