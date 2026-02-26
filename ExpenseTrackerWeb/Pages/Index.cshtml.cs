using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseTrackerWeb.Models;
using ExpenseTrackerWeb.Services;

namespace ExpenseTrackerWeb.Pages;

/// <summary>
/// Page model for the main expense tracker dashboard.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ExpenseApiService _apiService;

    public IndexModel(ExpenseApiService apiService)
    {
        _apiService = apiService;
    }

    public List<Expense> Expenses { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public MonthlyTotal? MonthlyTotal { get; set; }
    public List<CategorySummary> CategorySummaries { get; set; } = new();

    [BindProperty]
    public CreateExpenseRequest NewExpense { get; set; } = new();

    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Loads all dashboard data on GET requests.
    /// </summary>
    public async Task OnGetAsync()
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Handles the form submission to add a new expense.
    /// </summary>
    public async Task<IActionResult> OnPostAddExpenseAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }

        try
        {
            if (NewExpense.Date == null || NewExpense.Date == default(DateTime))
            {
                NewExpense.Date = DateTime.Now;
            }

            await _apiService.CreateExpenseAsync(NewExpense);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to add expense: {ex.Message}";
            await LoadDataAsync();
            return Page();
        }

        return RedirectToPage();
    }

    /// <summary>
    /// Handles the deletion of an expense by ID.
    /// </summary>
    public async Task<IActionResult> OnPostDeleteExpenseAsync(Guid id)
    {
        try
        {
            await _apiService.DeleteExpenseAsync(id);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete expense: {ex.Message}";
            await LoadDataAsync();
            return Page();
        }

        return RedirectToPage();
    }

    /// <summary>
    /// Fetches all data needed to render the dashboard.
    /// </summary>
    private async Task LoadDataAsync()
    {
        var expensesTask = _apiService.GetExpensesAsync();
        var categoriesTask = _apiService.GetCategoriesAsync();
        var monthlyTask = _apiService.GetMonthlyTotalAsync();
        var categoryTask = _apiService.GetCategorySummaryAsync();

        await Task.WhenAll(expensesTask, categoriesTask, monthlyTask, categoryTask);

        Expenses = expensesTask.Result;
        Categories = categoriesTask.Result;
        MonthlyTotal = monthlyTask.Result;
        CategorySummaries = categoryTask.Result;
    }
}
