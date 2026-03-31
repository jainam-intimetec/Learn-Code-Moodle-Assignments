using System.Globalization;

namespace Shared.Helpers;

public static class ConsoleHelper
{
    public static void ClearScreen()
    {
        try
        {
            if (!Console.IsOutputRedirected)
                Console.Clear();
        }
        catch (IOException)
        {
        }
        catch (InvalidOperationException)
        {
        }
    }

    public static void Pause(string message = "Press Enter to continue...")
    {
        Console.WriteLine();
        Console.Write(message);
        Console.ReadLine();
    }

    public static void WriteHeader(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(new string('=', title.Length + 8));
        Console.WriteLine($"   {title}");
        Console.WriteLine(new string('=', title.Length + 8));
        Console.ResetColor();
    }

    public static void WriteSuccess(string message)
    {
        WriteColoredLine(message, ConsoleColor.Green);
    }

    public static void WriteError(string message)
    {
        WriteColoredLine(message, ConsoleColor.Red);
    }

    public static void WriteInfo(string message)
    {
        WriteColoredLine(message, ConsoleColor.Yellow);
    }

    public static int PromptMenu(string prompt, params string[] options)
    {
        if (options.Length == 0)
            throw new InvalidOperationException("At least one menu option is required.");

        Console.WriteLine(prompt);
        for (var i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }

        return PromptInt("Select option", 1, options.Length);
    }

    public static string PromptRequiredText(string label, int minLength, int maxLength)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var value = Console.ReadLine()?.Trim() ?? string.Empty;

            if (value.Length < minLength)
            {
                WriteError($"{label} must be at least {minLength} characters long.");
                continue;
            }

            if (value.Length > maxLength)
            {
                WriteError($"{label} cannot exceed {maxLength} characters.");
                continue;
            }

            return value;
        }
    }

    public static string PromptOptionalText(string label, int maxLength)
    {
        while (true)
        {
            Console.Write($"{label} (optional): ");
            var value = Console.ReadLine()?.Trim() ?? string.Empty;

            if (value.Length > maxLength)
            {
                WriteError($"{label} cannot exceed {maxLength} characters.");
                continue;
            }

            return value;
        }
    }

    public static int PromptInt(string label, int min, int max)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                WriteError("Enter a valid whole number.");
                continue;
            }

            if (value < min || value > max)
            {
                WriteError($"Enter a number between {min} and {max}.");
                continue;
            }

            return value;
        }
    }

    public static decimal PromptDecimal(string label, decimal min, decimal max)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var input = Console.ReadLine();

            if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
            {
                WriteError("Enter a valid amount using digits only.");
                continue;
            }

            if (value < min || value > max)
            {
                WriteError($"Enter an amount between {min} and {max}.");
                continue;
            }

            return value;
        }
    }

    public static DateTime PromptDate(string label, DateTime defaultDate)
    {
        while (true)
        {
            Console.Write($"{label} [{defaultDate:yyyy-MM-dd}]: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return defaultDate.Date;

            if (!DateTime.TryParseExact(
                    input.Trim(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var value))
            {
                WriteError("Enter a valid date in yyyy-MM-dd format.");
                continue;
            }

            return value.Date;
        }
    }

    public static void WriteIndexedList(IEnumerable<string> items)
    {
        var index = 1;
        foreach (var item in items)
        {
            Console.WriteLine($"{index}. {item}");
            index++;
        }
    }

    private static void WriteColoredLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
