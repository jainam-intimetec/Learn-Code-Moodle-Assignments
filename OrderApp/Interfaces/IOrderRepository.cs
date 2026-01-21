using System.Threading.Tasks;

namespace OrderApp.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderById(string orderId);
        Task SaveOrder(Order order);
    }
}
