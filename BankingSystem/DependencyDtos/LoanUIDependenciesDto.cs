using BankingSystem.Interfaces;

namespace BankingSystem.DependencyDtos;

public sealed class LoanUiDependenciesDto
{
    public ILoanService LoanService { get; init; }
    public UiCommonServicesDto Ui { get; init; }
}
