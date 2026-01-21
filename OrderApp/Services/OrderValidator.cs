using OrderApp.Interfaces;

namespace OrderApp.Services
{
    public class OrderValidator : IOrderValidator
{
    public bool IsValid(Order order)
    {
        return HasItems(order) && HasValidTotal(order);
    }

    private static bool HasItems(Order order) =>
        order.Items?.Count > 0;

    private static bool HasValidTotal(Order order) =>
        order.TotalAmount > 0;
}
}
