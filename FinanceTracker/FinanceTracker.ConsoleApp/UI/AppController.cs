using FinanceTracker.Contracts.Auth;
using FinanceTracker.ConsoleApp.Configuration;

namespace FinanceTracker.ConsoleApp.UI;

public class AppController
{
    private readonly AuthScreen _authScreen;
    private readonly MainMenuScreen _mainMenuScreen;
    private readonly ApiOptions _apiOptions;

    public AppController(AuthScreen authScreen, MainMenuScreen mainMenuScreen, ApiOptions apiOptions)
    {
        _authScreen = authScreen;
        _mainMenuScreen = mainMenuScreen;
        _apiOptions = apiOptions;
    }

    public async Task RunAsync()
    {
        UserResponse? currentUser = null;

        while (true)
        {
            if (currentUser is null)
            {
                currentUser = await _authScreen.ShowAsync();
                if (currentUser is null)
                    return;
            }

            var shouldExit = await _mainMenuScreen.ShowAsync(currentUser, _apiOptions.BaseUrl);
            if (shouldExit)
                return;

            currentUser = null;
        }
    }
}
