using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace PRN221_Assignment.Authorization
{
    public class customAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
       
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null)
            {
                context.Result = new RedirectToPageResult("/login");
                return;
            }

        }
    }
}
