namespace Payment.Processing;

public class Wallet
{
    private double balance;

    public Wallet(double initialBalance)
    {
        balance = initialBalance;
    }

    public bool HasEnough(double amount)
    {
        return balance >= amount;
    }

    public void Debit(double amount)
    {
        balance -= amount;
    }
}