namespace Shared.Exceptions;

public class AuthenticationException : AppException
{
    public AuthenticationException(string message) : base(message)
    {
    }
}
