namespace ExpenseTrackerAPI.Models;

/// <summary>
/// Represents the total spending amount for the current month.
/// </summary>
public class MonthlyTotal
{
    /// <summary>Gets or sets the year of the summary.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the month of the summary.</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the total amount spent in the month.</summary>
    public decimal Total { get; set; }
}
