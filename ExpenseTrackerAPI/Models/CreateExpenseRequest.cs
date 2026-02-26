namespace ExpenseTrackerAPI.Models;

/// <summary>
/// Represents the request payload for creating a new expense.
/// </summary>
public class CreateExpenseRequest
{
    /// <summary>Gets or sets the monetary amount of the expense.</summary>
    public decimal Amount { get; set; }

    /// <summary>Gets or sets the category the expense belongs to.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Gets or sets an optional date for the expense. Defaults to the current UTC date/time when not provided.</summary>
    public DateTime? Date { get; set; }

    /// <summary>Gets or sets a short description of the expense.</summary>
    public string Description { get; set; } = string.Empty;
}
