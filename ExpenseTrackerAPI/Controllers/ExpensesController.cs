using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers;

/// <summary>
/// Handles HTTP requests for expense management.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    /// <summary>
    /// Initializes a new instance of <see cref="ExpensesController"/>.
    /// </summary>
    /// <param name="expenseService">The service used to manage expenses.</param>
    public ExpensesController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Retrieves all expenses.
    /// </summary>
    [HttpGet]
    public ActionResult<IReadOnlyList<Expense>> GetAll()
    {
        return Ok(_expenseService.GetAll());
    }

    /// <summary>
    /// Adds a new expense.
    /// </summary>
    /// <param name="request">The expense data.</param>
    [HttpPost]
    public ActionResult<Expense> Create([FromBody] CreateExpenseRequest request)
    {
        if (!ExpenseService.Categories.Contains(request.Category))
        {
            return BadRequest(new { error = $"Invalid category. Allowed values: {string.Join(", ", ExpenseService.Categories)}" });
        }

        var expense = _expenseService.Add(request);

        return CreatedAtAction(nameof(GetAll), new { id = expense.Id }, expense);
    }

    /// <summary>
    /// Deletes an expense by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the expense to delete.</param>
    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var deleted = _expenseService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Returns the total spending for the current month.
    /// </summary>
    [HttpGet("monthly-summary")]
    public ActionResult<object> GetMonthlySummary()
    {
        var total = _expenseService.GetCurrentMonthTotal();

        return Ok(new
        {
            year = DateTime.Today.Year,
            month = DateTime.Today.Month,
            total
        });
    }

    /// <summary>
    /// Returns the spending breakdown by category.
    /// </summary>
    [HttpGet("category-breakdown")]
    public ActionResult<Dictionary<string, decimal>> GetCategoryBreakdown()
    {
        return Ok(_expenseService.GetCategoryBreakdown());
    }

    /// <summary>
    /// Returns the fixed list of available expense categories.
    /// </summary>
    [HttpGet("categories")]
    public ActionResult<IReadOnlyList<string>> GetCategories()
    {
        return Ok(ExpenseService.Categories);
    }
}
