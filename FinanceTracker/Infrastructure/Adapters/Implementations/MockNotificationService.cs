using Infrastructure.Adapters.Interfaces;
using Infrastructure.Adapters.Models;

namespace Infrastructure.Adapters.Implementations;

public class MockNotificationService : INotificationService
{
    public Task<string> SendBudgetExceededAlertAsync(BudgetExceededAlert alert)
    {
        var message =
            $"Budget exceeded for {alert.Category} in {alert.Year:D4}-{alert.Month:D2}. Limit: {alert.LimitAmount:C}, Spent: {alert.SpentAmount:C}.";

        return Task.FromResult(message);
    }
}
