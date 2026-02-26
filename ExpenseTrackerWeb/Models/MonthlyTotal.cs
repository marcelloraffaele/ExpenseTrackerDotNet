namespace ExpenseTrackerWeb.Models;

/// <summary>
/// Represents the total spending for a given month and year.
/// </summary>
public class MonthlyTotal
{
    public int Year { get; set; }
    public int Month { get; set; }
    public double Total { get; set; }
}
