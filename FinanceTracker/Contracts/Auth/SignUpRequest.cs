namespace FinanceTracker.Contracts.Auth;

public class SignUpRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
