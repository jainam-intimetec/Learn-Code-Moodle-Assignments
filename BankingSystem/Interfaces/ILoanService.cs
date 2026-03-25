using BankingSystem.Models;

namespace BankingSystem.Interfaces;

public interface ILoanService
{
    Loan CalculateLoan(decimal principal, int tenure);
    void ApplyLoan(User user, Loan loan);
    void PayMonthlyEmi(User user);
    void SettleLoan(User user);
}
