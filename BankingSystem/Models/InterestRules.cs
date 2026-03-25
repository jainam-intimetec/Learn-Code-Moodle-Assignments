namespace BankingSystem.Models;

public class InterestRules
{
    public decimal MinPrincipal { get; set; }
    public decimal MaxPrincipal { get; set; }
    public int MinTenure { get; set; }
    public int MaxTenure { get; set; }
    public double InterestRate { get; set; }
}
