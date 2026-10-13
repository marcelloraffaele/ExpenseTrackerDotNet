using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseTrackerWeb.Pages;

public class IndexModel : PageModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public IndexModel(IConfiguration configuration)
    {
        ApiBaseUrl = configuration["ExpenseApi:BaseUrl"]
            ?? throw new InvalidOperationException("ExpenseApi:BaseUrl is not configured.");
    }

    /// <summary>
    /// Gets the base URL used by the page to call the expense API.
    /// </summary>
    public string ApiBaseUrl { get; }
}
