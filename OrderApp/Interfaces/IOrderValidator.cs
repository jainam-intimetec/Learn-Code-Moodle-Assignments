namespace OrderApp.Interfaces
{
    public interface IOrderValidator
    {
        bool IsOrderValid(Order order);
    }
}
