using System.Net.Http.Json;
using ExpenseTrackerWeb.Models;

namespace ExpenseTrackerWeb.Services;

/// <summary>
/// Service that wraps all calls to the ExpenseTracker API.
/// </summary>
public class ExpenseApiService
{
    private readonly HttpClient _httpClient;

    public ExpenseApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves all available expense categories.
    /// </summary>
    public async Task<List<string>> GetCategoriesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<string>>("/api/Expenses/categories");
        return result ?? new List<string>();
    }

    /// <summary>
    /// Retrieves all expenses.
    /// </summary>
    public async Task<List<Expense>> GetExpensesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Expense>>("/api/Expenses");
        return result ?? new List<Expense>();
    }

    /// <summary>
    /// Creates a new expense.
    /// </summary>
    public async Task<Expense?> CreateExpenseAsync(CreateExpenseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Expenses", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Expense>();
    }

    /// <summary>
    /// Deletes an expense by its ID.
    /// </summary>
    public async Task DeleteExpenseAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/Expenses/{id}");
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Retrieves the monthly spending total for the current month.
    /// </summary>
    public async Task<MonthlyTotal?> GetMonthlyTotalAsync()
    {
        return await _httpClient.GetFromJsonAsync<MonthlyTotal>("/api/Expenses/summary/monthly");
    }

    /// <summary>
    /// Retrieves a spending breakdown grouped by category.
    /// </summary>
    public async Task<List<CategorySummary>> GetCategorySummaryAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<CategorySummary>>("/api/Expenses/summary/categories");
        return result ?? new List<CategorySummary>();
    }
}
