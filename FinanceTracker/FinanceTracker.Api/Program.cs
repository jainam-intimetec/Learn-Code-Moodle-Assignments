using App.Services.Implementations;
using App.Services.Interfaces;
using FinanceTracker.Contracts.Auth;
using FinanceTracker.Contracts.Budgets;
using FinanceTracker.Contracts.Reports;
using FinanceTracker.Contracts.Transactions;
using Infrastructure.Adapters.Implementations;
using Infrastructure.Adapters.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<IBudgetRepository, BudgetRepository>();
builder.Services.AddSingleton<INotificationService, MockNotificationService>();

builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IBudgetService, BudgetService>();
builder.Services.AddSingleton<ITransactionService, TransactionService>();
builder.Services.AddSingleton<IReportService, ReportService>();

var app = builder.Build();

app.Urls.Clear();
app.Urls.Add("http://localhost:5057");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var (statusCode, message) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, exception.Message),
            AuthenticationException => (StatusCodes.Status401Unauthorized, exception.Message),
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, exception.Message),
            AppException => (StatusCodes.Status400BadRequest, exception?.Message ?? "Application error."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { message });
    });
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FinanceTracker API v1");
    options.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapPost("/api/auth/signup", async ([FromBody] SignUpRequest request, [FromServices] IAuthService authService) =>
{
    var user = await authService.SignUpAsync(request.UserName, request.Password);
    return Results.Ok(new UserResponse
    {
        Id = user.Id,
        UserName = user.UserName
    });
});

app.MapPost("/api/auth/login", async ([FromBody] LoginRequest request, [FromServices] IAuthService authService) =>
{
    var user = await authService.LoginAsync(request.UserName, request.Password);
    return Results.Ok(new UserResponse
    {
        Id = user.Id,
        UserName = user.UserName
    });
});

app.MapGet("/api/transactions", async ([FromQuery] Guid userId, [FromServices] ITransactionService transactionService) =>
{
    var transactions = await transactionService.GetByUserAsync(userId);
    return Results.Ok(transactions);
});

app.MapPost("/api/transactions", async ([FromBody] CreateTransactionRequest request, [FromServices] ITransactionService transactionService) =>
{
    var budgetAlertMessage = await transactionService.AddAsync(request);

    return Results.Created("/api/transactions", new CreateTransactionResponse
    {
        Message = "Transaction saved.",
        BudgetAlertMessage = budgetAlertMessage
    });
});

app.MapDelete("/api/transactions", async ([FromBody] DeleteTransactionRequest request, [FromServices] ITransactionService transactionService) =>
{
    await transactionService.DeleteAsync(request);
    return Results.NoContent();
});

app.MapGet("/api/budgets", async ([FromQuery] Guid userId, [FromServices] IBudgetService budgetService) =>
{
    var budgets = await budgetService.GetByUserAsync(userId);
    return Results.Ok(budgets);
});

app.MapPost("/api/budgets", async ([FromBody] UpsertBudgetRequest request, [FromServices] IBudgetService budgetService) =>
{
    await budgetService.AddOrUpdateAsync(request);
    return Results.Ok();
});

app.MapDelete("/api/budgets", async ([FromBody] DeleteBudgetRequest request, [FromServices] IBudgetService budgetService) =>
{
    await budgetService.DeleteAsync(request);
    return Results.NoContent();
});

app.MapGet("/api/reports/monthly", async (
    [FromQuery] Guid userId,
    [FromQuery] int year,
    [FromQuery] int month,
    [FromServices] IReportService reportService) =>
{
    var request = new MonthlyReportRequest
    {
        UserId = userId,
        Year = year,
        Month = month
    };

    var report = await reportService.GetMonthlyReportAsync(request);
    var budgets = await reportService.GetBudgetPerformanceAsync(request);

    return Results.Ok(new MonthlyReportResponse
    {
        Year = report.Year,
        Month = report.Month,
        TransactionCount = report.TransactionCount,
        TotalIncome = report.TotalIncome,
        TotalExpense = report.TotalExpense,
        Savings = report.Savings,
        Budgets = budgets.Select(b => new BudgetPerformanceResponse
        {
            Category = b.Category,
            BudgetAmount = b.BudgetAmount,
            SpentAmount = b.SpentAmount,
            RemainingAmount = b.RemainingAmount,
            IsOverBudget = b.IsOverBudget
        }).ToList()
    });
});

app.Run();
