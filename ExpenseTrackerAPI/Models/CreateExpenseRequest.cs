using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Models;

/// <summary>
/// Represents the request payload for creating a new expense.
/// </summary>
public class CreateExpenseRequest
{
    /// <summary>Gets or sets the monetary amount of the expense.</summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    /// <summary>Gets or sets the category of the expense.</summary>
    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    /// <summary>Gets or sets the optional date of the expense. Defaults to today if not provided.</summary>
    public DateOnly? Date { get; set; }

    /// <summary>Gets or sets the description of the expense.</summary>
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
