using System;
using System.Threading.Tasks;
using OrderApp.Interfaces;

namespace OrderApp.Services
{
   public class OrderCancellationService
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IInventoryService _inventoryService;
    private readonly IOrderRepository _orderRepository;

    public OrderCancellationService(
        IPaymentGateway paymentGateway,
        IInventoryService inventoryService,
        IOrderRepository orderRepository)
    {
        _paymentGateway = paymentGateway;
        _inventoryService = inventoryService;
        _orderRepository = orderRepository;
    }

    public async Task CancelOrder(string orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);

        if (OrderRequiresRefund(order))
        {
            await _paymentGateway.RefundPayment(order.TransactionId);
            await _inventoryService.RestoreInventory(order.Items);
        }

        order.Status = OrderStatus.Cancelled;
        await _orderRepository.SaveOrder(order);
    }

    private static bool OrderRequiresRefund(Order order) =>
        order.Status == OrderStatus.Paid;
}
}
