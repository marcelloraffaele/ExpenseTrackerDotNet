namespace ExpenseTrackerWeb.Models;

/// <summary>
/// Represents a single expense entry returned from the API.
/// </summary>
public class Expense
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
}
