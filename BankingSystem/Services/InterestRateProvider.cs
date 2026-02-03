using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.Services;

public class InterestRateProvider : IInterestRateProvider
{
    private readonly List<InterestRules> _rules;

    public InterestRateProvider()
    {
        _rules = new List<InterestRules>
        {
            new InterestRules
            {
                MinPrincipal = 0,
                MaxPrincipal = 50000,
                MinTenure = 1,
                MaxTenure = 12,
                InterestRate = 18
            },
            new InterestRules
            {
                MinPrincipal = 100001,
                MaxPrincipal = decimal.MaxValue,
                MinTenure = 24,
                MaxTenure = int.MaxValue,
                InterestRate = 5
            },
            new InterestRules
            {
                MinPrincipal = 0,
                MaxPrincipal = decimal.MaxValue,
                MinTenure = 1,
                MaxTenure = int.MaxValue,
                InterestRate = 10
            }
        };
    }

    public double GetInterestRate(decimal principal, int tenure)
    {
        var rule = _rules.First(r =>
            principal >= r.MinPrincipal &&
            principal <= r.MaxPrincipal &&
            tenure >= r.MinTenure &&
            tenure <= r.MaxTenure);

        return rule.InterestRate;
    }
}
