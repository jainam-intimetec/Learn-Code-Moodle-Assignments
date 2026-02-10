using BankingSystem.DependencyDtos;
using BankingSystem.Services;
using BankingSystem.UI;

var storageService = new FileStorageService();
var passwordHasher = new PasswordHasher();
var interestRateProvider = new InterestRateProvider();

var userService = new UserService(storageService, passwordHasher);
var accountService = new AccountService(storageService);
var transactionService = new TransactionService(storageService);
var loanService = new LoanService(storageService, interestRateProvider);

var uiCommonServices = new UiCommonServicesDto
{
    InputValidation = new InputValidationService(),
    Console = new ConsoleService()
};

var loanUiDependencies = new LoanUiDependenciesDto
{
    LoanService = loanService,
    Ui = uiCommonServices
};

var loanUI = new LoanUI(loanUiDependencies);

var dashboardDependencies = new DashboardUiDependenciesDto
{
    AccountService = accountService,
    TransactionService = transactionService,
    LoanUI = loanUI,
    Ui = uiCommonServices
};

var dashboardUI = new DashboardUI(dashboardDependencies);

var mainMenuDependencies = new MainMenuUiDependenciesDto
{
    UserService = userService,
    DashboardUI = dashboardUI,
    Ui = uiCommonServices
};

var mainMenu = new MainMenuUI(mainMenuDependencies);

mainMenu.Start();
