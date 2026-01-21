using System.Threading.Tasks;
using OrderApp.Interfaces;

namespace OrderApp.Services
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Order> GetOrderById(string orderId)
        {
            return Task.FromResult(new Order());
        }

        public Task SaveOrder(Order order)
        {
            return Task.CompletedTask;
        }
    }
}
