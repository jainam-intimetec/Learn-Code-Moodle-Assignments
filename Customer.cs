namespace Payment.Processing;

public class Customer
{
    private readonly string firstName;
    private readonly string lastName;
    private readonly Wallet wallet;

    public Customer(string firstName, string lastName, Wallet wallet)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.wallet = wallet;
    }

    public string GetFirstName()
    {
        return firstName;
    }

    public string GetLastName()
    {
        return lastName;
    }

    public bool Pay(double amount)
    {
        if (!wallet.HasEnough(amount))
            return false;

        wallet.Debit(amount);
        return true;
    }
}