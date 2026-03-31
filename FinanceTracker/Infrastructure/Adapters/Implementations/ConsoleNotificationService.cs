using Infrastructure.Adapters.Interfaces;

namespace Infrastructure.Adapters.Implementations;

public class ConsoleNotificationService : INotificationService
{
    public void Notify(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ALERT]: {message}");
        Console.ResetColor();
    }
}