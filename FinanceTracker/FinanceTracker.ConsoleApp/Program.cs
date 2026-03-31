using FinanceTracker.ConsoleApp.Configuration;
using FinanceTracker.ConsoleApp.UI;

namespace FinanceTracker.ConsoleApp;

public static class Program
{
    public static async Task Main()
    {
        var apiOptions = AppSettingsLoader.Load();
        var apiClient = new ApiClient(new HttpClient
        {
            BaseAddress = new Uri(apiOptions.BaseUrl)
        });

        var appController = new AppController(
            new AuthScreen(apiClient),
            new MainMenuScreen(apiClient),
            apiOptions);

        await appController.RunAsync();
    }
}
