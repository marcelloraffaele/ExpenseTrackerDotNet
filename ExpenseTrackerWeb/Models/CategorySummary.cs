namespace ExpenseTrackerWeb.Models;

/// <summary>
/// Represents the total spending for a single expense category.
/// </summary>
public class CategorySummary
{
    public string Category { get; set; } = string.Empty;
    public double Total { get; set; }
}
