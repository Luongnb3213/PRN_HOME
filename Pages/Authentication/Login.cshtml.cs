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
using PRN221_Assignment.Models;
using System.ComponentModel.DataAnnotations;
using static PRN221_Assignment.Pages.Authentication.LoginModel;
using Microsoft.EntityFrameworkCore;

namespace PRN221_Assignment.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly PRN221_Assignment.Respository.DataContext _context;

       

        private readonly IConfiguration _config;
        public LoginModel(IConfiguration config, PRN221_Assignment.Respository.DataContext context)
        {
            _config = config;
            _context = context;
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
            new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)),
            new Claim(ClaimTypes.Email, result.Principal.FindFirstValue(ClaimTypes.Email))
            };
            string name = result.Principal.FindFirstValue(ClaimTypes.Name);
            string email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var picture = result.Principal.FindFirstValue("urn:google:picture");


            //Account user = new Account();
            //if (!_context.Accounts.Any(a => a.Email == Input.Email))
            //{
            //    return RedirectToPage("/Authentication/Login");
            //}

            //else
            //{
            //    user = _context.Accounts.Where(x => x.Email == email).Include(x => x.Info).FirstOrDefault();
            //}











                // check xem email da ton tai chua, neu chua thi tao moi , co roi thi lay tu database ra


            //setCookies(user);

            return RedirectToPage("/Index");
        }

        [BindProperty]
        public UserLogin Input { get; set; }
        public IActionResult OnPost()
        {

            if (!_context.Accounts.Any(a => a.Email == Input.Email))
            {
                ModelState.AddModelError("Email", "This account doesn't exist in system ");
                return RedirectToPage("/Authentication/Login");

            }
            var user = Authenticate(Input);

            if (user == null)
            {
                ModelState.AddModelError("Password", "Password was wrong");
                return RedirectToPage("/Authentication/Login");
            }



            if (user != null)
            {
                setCookies(user);
                return RedirectToPage("/Index");
            }

            return Unauthorized();
        }
        private void setCookies(Account user)
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
        }

        

        private Account Authenticate(UserLogin userLogin)
        {
            Account acc = _context.Accounts.Where(x => x.Email == Input.Email && x.Password == Input.Password).Include(x => x.Info).FirstOrDefault();
            return acc;
        }

        private string GenerateJwtToken(Account user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JwtOptions:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtOptions:Issuer"],
                audience: _config["JwtOptions:Audience"],
                claims: new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Info?.Name),
                new Claim(ClaimTypes.Email, user?.Email),
                new Claim(ClaimTypes.Sid, user?.UserID+"")
                },
                expires: DateTime.Now.AddSeconds(Convert.ToInt32(_config["JwtOptions:ExpirationSeconds"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class UserLogin
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }


    }
}
