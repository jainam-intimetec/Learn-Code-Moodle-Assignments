using BankingSystem.Services;

namespace BankingSystem.DependencyDtos;

public sealed class UiCommonServicesDto
{
    public InputValidationService InputValidation { get; init; }
    public ConsoleService Console { get; init; }
}