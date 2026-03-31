using System.Net.Http.Json;
using Domain.Entities;
using FinanceTracker.Contracts.Auth;
using FinanceTracker.Contracts.Budgets;
using FinanceTracker.Contracts.Reports;
using FinanceTracker.Contracts.Transactions;

namespace FinanceTracker.ConsoleApp;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<UserResponse> SignUpAsync(string userName, string password) =>
        SendAsync<UserResponse>(HttpMethod.Post, "/api/auth/signup", new SignUpRequest
        {
            UserName = userName,
            Password = password
        });

    public Task<UserResponse> LoginAsync(string userName, string password) =>
        SendAsync<UserResponse>(HttpMethod.Post, "/api/auth/login", new LoginRequest
        {
            UserName = userName,
            Password = password
        });

    public Task<CreateTransactionResponse> AddTransactionAsync(CreateTransactionRequest request) =>
        SendAsync<CreateTransactionResponse>(HttpMethod.Post, "/api/transactions", request);

    public async Task<IReadOnlyList<Transaction>> GetTransactionsAsync(Guid userId) =>
        await GetAsync<List<Transaction>>($"/api/transactions?userId={userId}") ?? [];

    public Task DeleteTransactionAsync(DeleteTransactionRequest request) =>
        SendAsync(HttpMethod.Delete, "/api/transactions", request);

    public Task SaveBudgetAsync(UpsertBudgetRequest request) =>
        SendAsync(HttpMethod.Post, "/api/budgets", request);

    public async Task<IReadOnlyList<Budget>> GetBudgetsAsync(Guid userId) =>
        await GetAsync<List<Budget>>($"/api/budgets?userId={userId}") ?? [];

    public Task DeleteBudgetAsync(DeleteBudgetRequest request) =>
        SendAsync(HttpMethod.Delete, "/api/budgets", request);

    public Task<MonthlyReportResponse> GetMonthlyReportAsync(MonthlyReportRequest request) =>
        GetAsync<MonthlyReportResponse>(
            $"/api/reports/monthly?userId={request.UserId}&year={request.Year}&month={request.Month}");

    private async Task<T> GetAsync<T>(string uri)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<T>(uri)
                   ?? throw new InvalidOperationException("API response was empty.");
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException("Backend API is unavailable. Start FinanceTracker.Api first.");
        }
    }

    private async Task SendAsync(HttpMethod method, string uri, object? body = null)
    {
        try
        {
            using var request = new HttpRequestMessage(method, uri);
            if (body is not null)
                request.Content = JsonContent.Create(body);

            using var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return;

            throw new InvalidOperationException(await TryReadErrorAsync(response));
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException("Backend API is unavailable. Start FinanceTracker.Api first.");
        }
    }

    private async Task<T> SendAsync<T>(HttpMethod method, string uri, object? body = null)
    {
        try
        {
            using var request = new HttpRequestMessage(method, uri);
            if (body is not null)
                request.Content = JsonContent.Create(body);

            using var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(await TryReadErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<T>()
                   ?? throw new InvalidOperationException("API response was empty.");
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException("Backend API is unavailable. Start FinanceTracker.Api first.");
        }
    }

    private static async Task<string> TryReadErrorAsync(HttpResponseMessage response)
    {
        var payload = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        return payload?.Message ?? $"API request failed with status code {(int)response.StatusCode}.";
    }

    private class ApiErrorResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}
