namespace ExpenseTrackerAPI.Models;

/// <summary>
/// Represents a single expense entry.
/// </summary>
public class Expense
{
    /// <summary>Gets or sets the unique identifier for the expense.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the monetary amount of the expense.</summary>
    public decimal Amount { get; set; }

    /// <summary>Gets or sets the category the expense belongs to.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Gets or sets the date the expense occurred.</summary>
    public DateTime Date { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets a short description of the expense.</summary>
    public string Description { get; set; } = string.Empty;
}
