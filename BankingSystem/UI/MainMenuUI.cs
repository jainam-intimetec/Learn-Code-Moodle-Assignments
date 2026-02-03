using BankingSystem.Interfaces;
using BankingSystem.Models;
using BankingSystem.Services;

namespace BankingSystem.UI;

public class MainMenuUI
{
    private readonly IUserService _userService;
    private readonly DashboardUI _dashboard;
    private readonly InputValidationService _inputValidationService;
    private readonly ConsoleService _consoleService;

    public MainMenuUI(
        IUserService userService,
        DashboardUI dashboard,
        InputValidationService inputValidationService,
        ConsoleService consoleService)
    {
        _userService = userService;
        _dashboard = dashboard;
        _inputValidationService = inputValidationService;
        _consoleService = consoleService;
    }

    public void Start()
    {
        while (true)
        {
            try
            {
                ShowWelcomeScreen();
                var choice = ReadMenuChoice();

                if (choice == "3")
                    return;

                HandleMenuChoice(choice);
            }
            catch (Exception ex)
            {
                _consoleService.ShowError(ex.Message);
            }

            _consoleService.Pause();
        }
    }

    private void ShowWelcomeScreen()
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
        return _inputValidationService.RequireString(
            Console.ReadLine(),
            "Menu option");
    }

    private void HandleMenuChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                RegisterUser();
                break;
            case "2":
                Login();
                break;
            default:
                throw new Exception("Invalid menu option selected.");
        }
    }


    private void RegisterUser()
    {
        var user = new User();

        Console.Write("First Name: ");
        user.FirstName = _inputValidationService.RequireString(
            Console.ReadLine(), "First Name");

        Console.Write("Last Name: ");
        user.LastName = _inputValidationService.RequireString(
            Console.ReadLine(), "Last Name");

        Console.Write("Username: ");
        user.Username = _inputValidationService.RequireString(
            Console.ReadLine(), "Username");

        Console.Write("Password: ");
        var password = _consoleService.ReadPassword();

        Console.Write("Initial Deposit: ");
        user.Balance = _inputValidationService.RequireDecimal(
            Console.ReadLine(), "Initial Deposit");

        var createdUser = _userService.Register(user, password);

        Console.WriteLine();
        Console.WriteLine("Account created successfully.");
        Console.WriteLine($"Account ID: {createdUser.AccountId}");
    }


    private void Login()
    {
        Console.Write("Username: ");
        var username = _inputValidationService.RequireString(
            Console.ReadLine(), "Username");

        Console.Write("Password: ");
        var password = _consoleService.ReadPassword();

        var user = _userService.Login(username, password);
        _dashboard.Show(user);
    }
}
