using BankingSystem.Interfaces;
using BankingSystem.UI;

namespace BankingSystem.DependencyDtos;

public sealed class DashboardUiDependenciesDto
{
    public IAccountService AccountService { get; init; }
    public ITransactionService TransactionService { get; init; }
    public LoanUI LoanUI { get; init; }
    public UiCommonServicesDto Ui { get; init; }
}
