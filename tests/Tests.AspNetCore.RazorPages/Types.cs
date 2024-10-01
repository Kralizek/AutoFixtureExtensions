using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tests;

public class IndexPage : PageModel
{
    public IActionResult OnGet() => Page();
}