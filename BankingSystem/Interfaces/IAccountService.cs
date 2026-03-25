using BankingSystem.Models;

namespace BankingSystem.Interfaces;

public interface IAccountService
{
    void Deposit(User user, decimal amount);
    void Withdraw(User user, decimal amount);
}
