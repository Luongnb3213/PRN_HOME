using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
