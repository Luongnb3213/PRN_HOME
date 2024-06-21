using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN221_Assignment.Authorization;
using PRN221_Assignment.Models;
using System.Security.Claims;
using System.Threading;
namespace PRN221_Assignment.Pages.Shared.Components.MyViewComponent
{
    [ViewComponent(Name = "MyViewComponent")]
    public class MyViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = HttpContext.User as ClaimsPrincipal;
            var userId = user.FindFirst(c => c.Type == ClaimTypes.Sid)?.Value;
            return View("Default", userId);
        }
    }
}
