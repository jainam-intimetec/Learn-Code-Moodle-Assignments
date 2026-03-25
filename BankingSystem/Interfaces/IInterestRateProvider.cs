namespace BankingSystem.Interfaces;

public interface IInterestRateProvider
{
    double GetInterestRate(decimal principal, int tenure);
}
