using System;
using System.Threading.Tasks;
using OrderApp.Interfaces;

namespace OrderApp.Services
{
   public class OrderProcessor
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IInventoryService _inventoryService;
    private readonly INotificationService _notificationService;
    private readonly IErrorLogger _logger;
    private readonly IOrderValidator _orderValidator;

    public OrderProcessor(
        IPaymentGateway paymentGateway,
        IInventoryService inventoryService,
        INotificationService notificationService,
        IErrorLogger logger,
        IOrderValidator orderValidator)
    {
        _paymentGateway = paymentGateway ?? throw new ArgumentNullException(nameof(paymentGateway));
        _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderValidator = orderValidator ?? throw new ArgumentNullException(nameof(orderValidator));
    }

    public async Task<OrderResult> ProcessOrder(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (!_orderValidator.IsOrderValid(order))
            return OrderResult.Invalid("Order validation failed");

        if (!await _inventoryService.CheckAvailability(order.Items))
            return OrderResult.Failed("Insufficient inventory");

        await _inventoryService.ReserveItems(order.Items);

        try
        {
            return await ProcessPaymentAndFinalizeOrder(order);
        }
        catch (Exception ex)
        {
            await _inventoryService.ReleaseReservation(order.Items);
            _logger.Log(ex);
            throw;
        }
    }

    private async Task<OrderResult> ProcessPaymentAndFinalizeOrder(Order order)
    {
        var paymentResult = await _paymentGateway.ProcessPayment(
            order.CustomerId,
            order.TotalAmount,
            order.PaymentMethod);

        if (!paymentResult.IsSuccessful)
        {
            await _inventoryService.ReleaseReservation(order.Items);
            return OrderResult.Failed($"Payment failed: {paymentResult.ErrorMessage}");
        }

        await _inventoryService.CommitReservation(order.Items);
        await _notificationService.SendOrderConfirmation(order);

        return OrderResult.Success(paymentResult.TransactionId);
    }
}

}
