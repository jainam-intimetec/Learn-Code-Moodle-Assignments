using BankingSystem.Services;
using BankingSystem.UI;

var storageService = new FileStorageService();
var inputValidationService = new InputValidationService();
var consoleService = new ConsoleService();
var passwordHasher = new PasswordHasher();

var userService = new UserService(
    storageService,
    passwordHasher);

var accountService = new AccountService(storageService);
var transactionService = new TransactionService(storageService);
var loanService = new LoanService(storageService, new InterestRateProvider());


var loanUI = new LoanUI(
    loanService,
    inputValidationService,
    consoleService);

var dashboardUI = new DashboardUI(
    accountService,
    transactionService,
    loanUI,
    inputValidationService,
    consoleService);

var mainMenu = new MainMenuUI(
    userService,
    dashboardUI,
    inputValidationService,
    consoleService);

mainMenu.Start();
