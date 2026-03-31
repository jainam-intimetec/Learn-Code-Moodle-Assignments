using Infrastructure.Adapters.Models;

namespace Infrastructure.Adapters.Interfaces;

public interface INotificationService
{
    Task<string> SendBudgetExceededAlertAsync(BudgetExceededAlert alert);
}
