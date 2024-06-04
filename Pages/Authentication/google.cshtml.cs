using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN221_Assignment.Pages.Authentication
{
    public class googleModel : PageModel
    {
        private readonly IConfiguration _config;
        public googleModel(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult OnGet()
        {
            var redirectUrl = Url.Page("/authentication/google", pageHandler: "Callback");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }
        public async Task<IActionResult> OnGetCallbackAsync()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                // Authentication failed
                return RedirectToPage("/Error");
            }

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, result.Principal.FindFirstValue(ClaimTypes.NameIdentifier)),
            new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)),
            new Claim(ClaimTypes.Email, result.Principal.FindFirstValue(ClaimTypes.Email))
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtOptions:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["JwtOptions:Issuer"],
                audience: _config["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            HttpContext.Response.Cookies.Append("jwtToken", jwt, new CookieOptions { HttpOnly = true });

            return RedirectToPage("/Index");
        }
    }
}
