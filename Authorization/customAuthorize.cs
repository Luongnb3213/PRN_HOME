using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace PRN221_Assignment.Authorization
{
    public class customAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
       
        private readonly string role;

        public customAuthorize( string roles )
        {
            role = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.Name.Equals(role))
            {
                return;
            }

            context.Result = new ForbidResult();
            return;
        }
    }
}
