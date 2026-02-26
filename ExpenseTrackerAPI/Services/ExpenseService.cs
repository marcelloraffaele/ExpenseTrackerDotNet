using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Services;

/// <summary>
/// Provides in-memory storage and business logic for expense management.
/// </summary>
public class ExpenseService
{
    /// <summary>The fixed list of available expense categories.</summary>
    public static readonly IReadOnlyList<string> Categories = new List<string>
    {
        "Food",
        "Transport",
        "Entertainment",
        "Shopping",
        "Bills",
        "Health",
        "Education",
        "Travel",
        "Personal Care",
        "Home"
    };

    private readonly List<Expense> _expenses = new();
    private readonly Lock _lock = new();

    /// <summary>
    /// Returns all expenses stored in memory.
    /// </summary>
    public IReadOnlyList<Expense> GetAll()
    {
        lock (_lock)
        {
            return _expenses.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Adds a new expense to the in-memory list.
    /// </summary>
    /// <param name="request">The request containing expense data.</param>
    /// <returns>The newly created expense.</returns>
    public Expense Add(CreateExpenseRequest request)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Category = request.Category,
            Date = request.Date ?? DateOnly.FromDateTime(DateTime.Today),
            Description = request.Description
        };

        lock (_lock)
        {
            _expenses.Add(expense);
        }

        return expense;
    }

    /// <summary>
    /// Deletes an expense by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the expense to delete.</param>
    /// <returns>True if the expense was found and removed; otherwise false.</returns>
    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);

            if (expense is null)
            {
                return false;
            }

            _expenses.Remove(expense);

            return true;
        }
    }

    /// <summary>
    /// Returns the total spending amount for the current calendar month.
    /// </summary>
    public decimal GetCurrentMonthTotal()
    {
        var today = DateTime.Today;

        lock (_lock)
        {
            return _expenses
                .Where(e => e.Date.Year == today.Year && e.Date.Month == today.Month)
                .Sum(e => e.Amount);
        }
    }

    /// <summary>
    /// Returns the total spending grouped by category.
    /// </summary>
    public Dictionary<string, decimal> GetCategoryBreakdown()
    {
        lock (_lock)
        {
            return _expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }
    }
}
