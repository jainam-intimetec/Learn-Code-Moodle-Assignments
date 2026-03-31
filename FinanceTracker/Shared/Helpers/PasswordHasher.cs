using System.Security.Cryptography;
using System.Text;

namespace Shared.Helpers;

public static class PasswordHasher
{
    public static string Hash(string rawValue)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(rawValue);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}
