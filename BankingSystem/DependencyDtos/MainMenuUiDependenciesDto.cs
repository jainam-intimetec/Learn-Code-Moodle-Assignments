using BankingSystem.Interfaces;
using BankingSystem.UI;

namespace BankingSystem.DependencyDtos;

public sealed class MainMenuUiDependenciesDto
{
    public IUserService UserService { get; init; }
    public DashboardUI DashboardUI { get; init; }
    public UiCommonServicesDto Ui { get; init; }
}
