using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Authorization;
using System.Security.Claims;

namespace PRN221_Assignment.Pages
{
    [customAuthorize("test")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var username = User.Identity;
            var isAuthenticate = User.Identity?.IsAuthenticated ?? false;
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return new JsonResult(new { username, isAuthenticate, email });
        }
    }
}
