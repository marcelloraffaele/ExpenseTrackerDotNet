using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers;

/// <summary>
/// Handles all expense management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    /// <summary>
    /// Initializes a new instance of <see cref="ExpensesController"/>.
    /// </summary>
    /// <param name="expenseService">The injected expense service.</param>
    public ExpensesController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Returns the fixed list of available expense categories.
    /// </summary>
    /// <returns>A list of category name strings.</returns>
    [HttpGet("categories")]
    public ActionResult<IReadOnlyList<string>> GetCategories()
    {
        return Ok(ExpenseService.Categories);
    }

    /// <summary>
    /// Returns all stored expenses.
    /// </summary>
    /// <returns>A list of all <see cref="Expense"/> entries.</returns>
    [HttpGet]
    public ActionResult<IReadOnlyList<Expense>> GetAll()
    {
        return Ok(_expenseService.GetAll());
    }

    /// <summary>
    /// Creates a new expense entry.
    /// </summary>
    /// <param name="request">The expense data to add.</param>
    /// <returns>The newly created <see cref="Expense"/>.</returns>
    [HttpPost]
    public ActionResult<Expense> Create([FromBody] CreateExpenseRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest("Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.Category))
        {
            return BadRequest("Category is required.");
        }

        if (!ExpenseService.Categories.Contains(request.Category))
        {
            return BadRequest($"Invalid category. Valid categories are: {string.Join(", ", ExpenseService.Categories)}.");
        }

        var expense = _expenseService.Add(request);

        return CreatedAtAction(nameof(GetAll), new { }, expense);
    }

    /// <summary>
    /// Deletes the expense with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the expense to delete.</param>
    /// <returns>No content on success, or 404 if not found.</returns>
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _expenseService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Returns the total spending for the current calendar month.
    /// </summary>
    /// <returns>A <see cref="MonthlyTotal"/> with year, month, and total amount.</returns>
    [HttpGet("summary/monthly")]
    public ActionResult<MonthlyTotal> GetMonthlyTotal()
    {
        return Ok(_expenseService.GetCurrentMonthTotal());
    }

    /// <summary>
    /// Returns a spending breakdown grouped by category.
    /// </summary>
    /// <returns>A list of <see cref="CategorySummary"/> entries ordered by total descending.</returns>
    [HttpGet("summary/categories")]
    public ActionResult<IReadOnlyList<CategorySummary>> GetCategoryBreakdown()
    {
        return Ok(_expenseService.GetCategoryBreakdown());
    }
}
