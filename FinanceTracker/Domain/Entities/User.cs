using Shared.Exceptions;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }

    public static User Create(string userName, string passwordHash)
    {
        Validate(userName, passwordHash);

        return new User
        {
            Id = Guid.NewGuid(),
            UserName = userName.Trim(),
            PasswordHash = passwordHash,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static void Validate(string userName, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(userName) || userName.Trim().Length < 3)
            throw new ValidationException("Username must be at least 3 characters long.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ValidationException("Password hash cannot be empty.");
    }
}
