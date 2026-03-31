using FinanceTracker.Contracts.Auth;
using Shared.Helpers;

namespace FinanceTracker.ConsoleApp.UI;

public class AuthScreen
{
    private readonly ApiClient _client;

    public AuthScreen(ApiClient client)
    {
        _client = client;
    }

    public async Task<UserResponse?> ShowAsync()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("Finance Tracker Console");
            ConsoleHelper.WriteHeader("Login / Sign Up");

            var choice = ConsoleHelper.PromptMenu("Choose an option:", "Login", "Sign up", "Exit");

            try
            {
                switch (choice)
                {
                    case 1:
                        return await _client.LoginAsync(
                            ConsoleHelper.PromptRequiredText("Username", 3, 30),
                            ConsoleHelper.PromptRequiredText("Password", 6, 100));
                    case 2:
                        return await _client.SignUpAsync(
                            ConsoleHelper.PromptRequiredText("Choose username", 3, 30),
                            ConsoleHelper.PromptRequiredText("Choose password", 6, 100));
                    case 3:
                        ConsoleHelper.WriteInfo("Goodbye.");
                        return null;
                }
            }
            catch (InvalidOperationException ex)
            {
                ConsoleHelper.WriteError(ex.Message);
                ConsoleHelper.Pause();
            }
        }
    }
}
