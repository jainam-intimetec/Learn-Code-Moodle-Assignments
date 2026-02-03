using BankingSystem.Interfaces;
using BankingSystem.Models;

namespace BankingSystem.Services;

public class UserService : IUserService
{
    private readonly IFileStorageService _storage;
    private readonly PasswordHasher _passwordHasher;

    public UserService(
        IFileStorageService storage,
        PasswordHasher passwordHasher)
    {
        _storage = storage;
        _passwordHasher = passwordHasher;
    }

    public User Register(User user, string password)
    {
        ValidateInitialDeposit(user);
        EnsureUsernameIsUnique(user.Username);

        user.AccountId = GenerateAccountId();
        user.PasswordHash = _passwordHasher.Hash(password);

        var users = _storage.LoadUsers();
        users.Add(user);
        _storage.SaveUsers(users);

        return user;
    }

    public User Login(string username, string password)
    {
        var users = _storage.LoadUsers();
        var hash = _passwordHasher.Hash(password);

        var user = users.FirstOrDefault(u =>
            u.Username == username && u.PasswordHash == hash);

        if (user == null)
            throw new Exception("Invalid username or password.");

        return user;
    }


    private void ValidateInitialDeposit(User user)
    {
        if (user.Balance < 500)
            throw new Exception("Minimum deposit is 500 Rs.");
    }

    private void EnsureUsernameIsUnique(string username)
    {
        var users = _storage.LoadUsers();

        if (users.Any(u => u.Username == username))
            throw new Exception("Username already exists.");
    }

    private string GenerateAccountId()
    {
        return Guid.NewGuid().ToString("N")[..10];
    }
}
