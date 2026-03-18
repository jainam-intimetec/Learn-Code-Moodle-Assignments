namespace Payment.Processing;

public class Paperboy
{
    public void CollectPayment(Customer customer, double paymentAmount)
    {
        var isPaid = customer.Pay(paymentAmount);

        if (!isPaid)
        {
            // come back later
        }
    }
}