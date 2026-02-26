namespace ExpenseTrackerAPI.Models;

/// <summary>
/// Represents an expense entry in the tracker.
/// </summary>
public class Expense
{
    /// <summary>Gets or sets the unique identifier of the expense.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the monetary amount of the expense.</summary>
    public decimal Amount { get; set; }

    /// <summary>Gets or sets the category of the expense.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Gets or sets the date of the expense.</summary>
    public DateOnly Date { get; set; }

    /// <summary>Gets or sets the description of the expense.</summary>
    public string Description { get; set; } = string.Empty;
}
