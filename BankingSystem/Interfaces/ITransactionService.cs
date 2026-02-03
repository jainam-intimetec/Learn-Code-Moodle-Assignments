using BankingSystem.Models;

namespace BankingSystem.Interfaces;

public interface ITransactionService
{
    void Transfer(User sender, string targetAccountId, decimal amount);
}
