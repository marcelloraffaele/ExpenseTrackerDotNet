namespace ExpenseTrackerWeb.Models;

/// <summary>
/// Represents the request body for creating a new expense.
/// </summary>
public class CreateExpenseRequest
{
    public double Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string Description { get; set; } = string.Empty;
}
