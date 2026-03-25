using System.Text;

namespace BankingSystem.Services;

public class ConsoleService
{
    public string ReadPassword()
    {
        var password = CapturePasswordInput();

        EnsureNotEmpty(password);

        return password;
    }

    public void ShowError(string message)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }

    public void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static string CapturePasswordInput()
    {
        var password = new StringBuilder();

        while (true)
        {
            var key = ReadKey();

            if (IsEnter(key))
                break;

            if (IsBackspace(key))
            {
                HandleBackspace(password);
                continue;
            }

            if (IsPrintableCharacter(key))
            {
                AppendCharacter(password, key.KeyChar);
            }
        }

        Console.WriteLine();
        return password.ToString();
    }

    private static ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey(intercept: true);
    }

    private static bool IsEnter(ConsoleKeyInfo key)
    {
        return key.Key == ConsoleKey.Enter;
    }

    private static bool IsBackspace(ConsoleKeyInfo key)
    {
        return key.Key == ConsoleKey.Backspace;
    }

    private static bool IsPrintableCharacter(ConsoleKeyInfo key)
    {
        return !char.IsControl(key.KeyChar);
    }

    private static void HandleBackspace(StringBuilder password)
    {
        if (password.Length == 0)
            return;

        password.Remove(password.Length - 1, 1);
        Console.Write("\b \b");
    }

    private static void AppendCharacter(StringBuilder password, char character)
    {
        password.Append(character);
        Console.Write("*");
    }

    private static void EnsureNotEmpty(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Password cannot be empty.");
    }


}
