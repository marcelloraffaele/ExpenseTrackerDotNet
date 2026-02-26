namespace ExpenseTrackerAPI.Models;

/// <summary>
/// Represents the total spending amount for a single category.
/// </summary>
public class CategorySummary
{
    /// <summary>Gets or sets the expense category name.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Gets or sets the total amount spent in this category.</summary>
    public decimal Total { get; set; }
}
