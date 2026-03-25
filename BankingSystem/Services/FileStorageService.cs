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
        try
        {
            EnsureStorageExists();

            var json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<User>();

            return JsonSerializer.Deserialize<List<User>>(json, SerializerOptions)
                   ?? new List<User>();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Stored user data is corrupted.", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException("Unable to read stored user data.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new InvalidOperationException("Access to the user data file was denied.", ex);
        }
    }

    public void SaveUsers(List<User> users)
    {
        try
        {
            EnsureStorageExists();

            var json = JsonSerializer.Serialize(users, SerializerOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException("Unable to save user data.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new InvalidOperationException("Access to the user data file was denied.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new InvalidOperationException("The configured storage path is invalid.", ex);
        }
    }

    private void EnsureStorageExists()
    {
        try
        {
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException("Unable to initialize application storage.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new InvalidOperationException("Access to the application storage location was denied.", ex);
        }
    }
}
