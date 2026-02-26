using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Services;

/// <summary>
/// In-memory service for managing expense entries.
/// </summary>
public class ExpenseService
{
    /// <summary>
    /// The fixed list of valid expense categories.
    /// </summary>
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
        "Housing",
        "Personal Care"
    };

    private readonly List<Expense> _expenses = new();

    /// <summary>
    /// Returns all stored expenses.
    /// </summary>
    public IReadOnlyList<Expense> GetAll()
    {
        return _expenses.AsReadOnly();
    }

    /// <summary>
    /// Adds a new expense from the provided request data.
    /// </summary>
    /// <param name="request">The data for the new expense.</param>
    /// <returns>The newly created expense.</returns>
    public Expense Add(CreateExpenseRequest request)
    {
        var expense = new Expense
        {
            Amount = request.Amount,
            Category = request.Category,
            Date = request.Date ?? DateTime.UtcNow,
            Description = request.Description
        };

        _expenses.Add(expense);

        return expense;
    }

    /// <summary>
    /// Deletes the expense with the given identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the expense to delete.</param>
    /// <returns><c>true</c> if the expense was found and removed; otherwise <c>false</c>.</returns>
    public bool Delete(Guid id)
    {
        var expense = _expenses.FirstOrDefault(e => e.Id == id);

        if (expense is null)
        {
            return false;
        }

        _expenses.Remove(expense);

        return true;
    }

    /// <summary>
    /// Calculates the total spending for the current calendar month (UTC).
    /// </summary>
    /// <returns>A <see cref="MonthlyTotal"/> with the year, month, and summed amount.</returns>
    public MonthlyTotal GetCurrentMonthTotal()
    {
        var now = DateTime.UtcNow;

        var total = _expenses
            .Where(e => e.Date.Year == now.Year && e.Date.Month == now.Month)
            .Sum(e => e.Amount);

        return new MonthlyTotal
        {
            Year = now.Year,
            Month = now.Month,
            Total = total
        };
    }

    /// <summary>
    /// Returns the total spending grouped by category.
    /// </summary>
    /// <returns>A list of <see cref="CategorySummary"/> entries, one per category that has expenses.</returns>
    public IReadOnlyList<CategorySummary> GetCategoryBreakdown()
    {
        var breakdown = _expenses
            .GroupBy(e => e.Category)
            .Select(g => new CategorySummary
            {
                Category = g.Key,
                Total = g.Sum(e => e.Amount)
            })
            .OrderByDescending(c => c.Total)
            .ToList();

        return breakdown.AsReadOnly();
    }
}
