using System.Text.Json;

namespace Shared.Helpers;

public static class JsonStorageHelper
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static async Task<List<T>> ReadAsync<T>(string path)
    {
        EnsureFileExists(path);

        var json = await File.ReadAllTextAsync(path);
        if (string.IsNullOrWhiteSpace(json))
            return new List<T>();

        return JsonSerializer.Deserialize<List<T>>(json, SerializerOptions) ?? new List<T>();
    }

    public static async Task WriteAsync<T>(string path, List<T> data)
    {
        EnsureFileExists(path);

        var json = JsonSerializer.Serialize(data, SerializerOptions);
        await File.WriteAllTextAsync(path, json);
    }

    private static void EnsureFileExists(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);

        if (!File.Exists(path))
            File.WriteAllText(path, "[]");
    }
}
