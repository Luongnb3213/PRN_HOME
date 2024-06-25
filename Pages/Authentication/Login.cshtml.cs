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


            string name = result.Principal.FindFirstValue(ClaimTypes.Name);
            string email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var picture = result.Principal.FindFirstValue("urn:google:picture");
            string userName = name.Replace(" ", "_");

            Account user = new Account();
            if (!_context.Accounts.Any(a => a.Email == email))
            {
                // create new account
                try
                {
                    user.Email = email;
                    user.Password = "default Password";
                    _context.Accounts.Add(user);
                    _context.SaveChanges();
                    Info info = new Info
                    {
                        UserID = user.UserID,
                        Dob = DateTime.Now,
                        Image = picture,
                        Name = name,
                        Story = "",
                        userName = userName,
                    };
                    _context.Info.Add(info);
                    _context.SaveChanges();
                    user.Info = info;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Page();
                }
            }

            else
            {
                user = _context.Accounts.Where(x => x.Email == email).Include(x => x.Info).FirstOrDefault();
            }
            if (user != null)
            {
                //var claims = new[]
                // {
                //    new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)),
                //    new Claim(ClaimTypes.Email, result.Principal.FindFirstValue(ClaimTypes.Email))
                // };
                //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JwtOptions:SigningKey"]));
                //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //var token = new JwtSecurityToken(
                //                    issuer: _config["JwtOptions:Issuer"],
                //                    audience: _config["JwtOptions:Audience"],
                //                    claims: claims,
                //                   expires: DateTime.Now.AddSeconds(Convert.ToInt32(_config["JwtOptions:ExpirationSeconds"])),
                //                    signingCredentials: creds);
                //var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                //HttpContext.Response.Cookies.Append("jwtToken", jwt, new CookieOptions { HttpOnly = true });
                setCookies(user);
            }


            return RedirectToPage("/Index");
        }

        [BindProperty]
        public UserLogin Input { get; set; }
        public IActionResult OnPost()
        {

            if (!_context.Accounts.Any(a => a.Email.Equals(Input.Email)))
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
                Expires = expiration
            };

            HttpContext.Response.Cookies.Append("jwtToken", token, cookieOptions);
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
                new Claim(ClaimTypes.NameIdentifier,  user?.UserID+""),
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
