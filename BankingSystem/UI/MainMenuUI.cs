using BankingSystem.DependencyDtos;
using BankingSystem.Models;

namespace BankingSystem.UI;

public class MainMenuUI
{
    private readonly MainMenuUiDependenciesDto _deps;

    public MainMenuUI(MainMenuUiDependenciesDto dependencies)
    {
        _deps = dependencies;
    }

    public void Start()
    {
        while (true)
        {
            try
            {
                ShowWelcome();
                var choice = ReadMenuChoice();

                if (choice == "3")
                    return;

                HandleChoice(choice);
            }
            catch (Exception ex)
            {
                _deps.Ui.Console.ShowError(ex.Message);
            }

            _deps.Ui.Console.Pause();
        }
    }

    private void ShowWelcome()
    {
        Console.Clear();
        Console.WriteLine("Welcome To JJ Bank.");
        Console.WriteLine("Your Money is our Money and our Money is ours.");
        Console.WriteLine("===============================================");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("3. Exit");
    }

    private string ReadMenuChoice()
    {
        Console.Write("Please select an option: ");
        return _deps.Ui.InputValidation.RequireString(
            Console.ReadLine(), "Menu option");
    }

    private void HandleChoice(string choice)
    {
        switch (choice)
        {
            case "1": Register(); break;
            case "2": Login(); break;
            default:
                throw new InvalidOperationException("Invalid menu option selected.");
        }
    }

    private void Register()
    {
        var user = new User();

        Console.Write("First Name: ");
        user.FirstName = _deps.Ui.InputValidation.RequireString(Console.ReadLine(), "First Name");

        Console.Write("Last Name: ");
        user.LastName = _deps.Ui.InputValidation.RequireString(Console.ReadLine(), "Last Name");

        Console.Write("Username: ");
        user.Username = _deps.Ui.InputValidation.RequireString(Console.ReadLine(), "Username");

        Console.Write("Password: ");
        var password = _deps.Ui.Console.ReadPassword();

        Console.Write("Initial Deposit: ");
        user.Balance = _deps.Ui.InputValidation.RequireDecimal(Console.ReadLine(), "Initial Deposit");

        var createdUser = _deps.UserService.Register(user, password);

        Console.WriteLine($"Account created successfully. Account ID: {createdUser.AccountId}");
    }

    private void Login()
    {
        Console.Write("Username: ");
        var username = _deps.Ui.InputValidation.RequireString(Console.ReadLine(), "Username");

        Console.Write("Password: ");
        var password = _deps.Ui.Console.ReadPassword();

        var user = _deps.UserService.Login(username, password);
        _deps.DashboardUI.Show(user);
    }
}
