using System.Text.Json;

namespace FinanceTracker.ConsoleApp.Configuration;

public static class AppSettingsLoader
{
    public static ApiOptions Load()
    {
        var path = ResolveSettingsPath();
        if (path is null || !File.Exists(path))
            return new ApiOptions();

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<ApiOptions>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new ApiOptions();
    }

    private static string? ResolveSettingsPath()
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

        while (currentDirectory is not null)
        {
            var candidate = Path.Combine(currentDirectory.FullName, "appsettings.console.json");
            if (File.Exists(candidate))
                return candidate;

            currentDirectory = currentDirectory.Parent;
        }

        return null;
    }
}
