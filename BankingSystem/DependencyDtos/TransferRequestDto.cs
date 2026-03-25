namespace BankingSystem.Models;

public sealed class TransferRequest
{
    public User Sender { get; init; }
    public string TargetAccountId { get; init; }
    public decimal Amount { get; init; }
}
