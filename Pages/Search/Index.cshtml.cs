using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using static PRN221_Assignment.Pages.mess.IndexModel;

namespace PRN221_Assignment.Pages.Search
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostSearchValue([FromBody] string data)
        {
            return new JsonResult(new { data = data });
        }
    }
}
