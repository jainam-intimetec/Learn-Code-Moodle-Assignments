using System.Security.Cryptography;
using System.Text;

namespace BankingSystem.Services;

public class PasswordHasher
{
    public string Hash(string password)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(
            sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}
