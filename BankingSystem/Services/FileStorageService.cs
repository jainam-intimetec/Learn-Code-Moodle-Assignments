using System.Text.Json;
using System.Text.Json.Serialization;
using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _folderPath;
    private readonly string _filePath;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public FileStorageService()
    {
        _folderPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Data");

        _filePath = Path.Combine(_folderPath, "users.json");

        EnsureStorageExists();
    }

    public List<User> LoadUsers()
    {
        EnsureStorageExists();

        var json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<User>();

        return JsonSerializer.Deserialize<List<User>>(json, SerializerOptions)
               ?? new List<User>();
    }

    public void SaveUsers(List<User> users)
    {
        EnsureStorageExists();

        var json = JsonSerializer.Serialize(users, SerializerOptions);
        File.WriteAllText(_filePath, json);
    }

    private void EnsureStorageExists()
    {
        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

        if (!File.Exists(_filePath))
            File.WriteAllText(_filePath, "[]");
    }
}
