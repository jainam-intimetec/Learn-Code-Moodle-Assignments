namespace BankingSystem.Services;

public class InputValidationService
{
    public string RequireString(string? input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new Exception($"{fieldName} cannot be empty.");

        return input.Trim();
    }

    public decimal RequireDecimal(string? input, string fieldName)
    {
        if (!decimal.TryParse(input, out var value))
            throw new Exception($"{fieldName} must be a valid number.");

        return value;
    }
}
