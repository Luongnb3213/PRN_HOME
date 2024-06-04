using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

namespace PRN221_Assignment.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _config;
        public LoginModel(IConfiguration config)
        {
            _config = config;
        }
        public void OnGet()
        {
        }
        public IActionResult OnGetIndex()
        {
            return RedirectToPage("/Index"); 
        }
        public IActionResult OnGetLogingoogle()
        {
            var redirectUrl = Url.Page("/authentication/login", pageHandler: "Callback");
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

        [BindProperty]
        public UserLogin Input { get; set; }
        public IActionResult OnPost()
        {
            var user = Authenticate(Input);

            if (user != null)
            {
                var token = GenerateJwtToken(user);
                var expiration = DateTime.UtcNow.AddSeconds(Convert.ToInt32(_config["JwtOptions:ExpirationSeconds"]));
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = expiration
                };
           
                Response.Cookies.Append("jwtToken", token, cookieOptions);
                return RedirectToPage("/Index");
            }

            return Unauthorized();
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            if (userLogin.Username == "test1" && userLogin.Password == "123456")
            {
                return new UserModel { Username = "test1", Email = "luongnb3213@gmail.com" };
            }

            return null;
        }

        private string GenerateJwtToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JwtOptions:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtOptions:Issuer"],
                audience: _config["JwtOptions:Audience"],
                claims: new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, "0912800141")
                },
                expires: DateTime.Now.AddSeconds(Convert.ToInt32(_config["JwtOptions:ExpirationSeconds"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class UserLogin
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class UserModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }
    }
}
