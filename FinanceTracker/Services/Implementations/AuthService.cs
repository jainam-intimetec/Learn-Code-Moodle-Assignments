using App.Services.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Shared.Exceptions;
using Shared.Helpers;

namespace App.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> LoginAsync(string userName, string password)
    {
        ValidateCredentials(userName, password);

        var user = await _userRepository.GetByUserNameAsync(userName);
        if (user is null)
            throw new AuthenticationException("Invalid username or password.");

        var passwordHash = PasswordHasher.Hash(password);
        if (!string.Equals(user.PasswordHash, passwordHash, StringComparison.Ordinal))
            throw new AuthenticationException("Invalid username or password.");

        return user;
    }

    public async Task<User> SignUpAsync(string userName, string password)
    {
        ValidateCredentials(userName, password);

        var existingUser = await _userRepository.GetByUserNameAsync(userName);
        if (existingUser is not null)
            throw new ConflictException("Username already exists. Please choose another username.");

        var user = User.Create(userName, PasswordHasher.Hash(password));
        await _userRepository.AddAsync(user);
        return user;
    }

    private static void ValidateCredentials(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName) || userName.Trim().Length < 3)
            throw new ValidationException("Username must be at least 3 characters long.");

        if (string.IsNullOrWhiteSpace(password) || password.Trim().Length < 6)
            throw new ValidationException("Password must be at least 6 characters long.");
    }
}
