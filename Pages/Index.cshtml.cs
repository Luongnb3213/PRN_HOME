using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Authorization;
using PRN221_Assignment.Models;
using System.Security.Claims;

namespace PRN221_Assignment.Pages
{
      
    public class IndexModel : PageModel
    {
        private readonly PRN221_Assignment.Respository.DataContext context;

        public IndexModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
            //Thread = new Models.Thread();
        }
        [BindProperty]
        public Models.Thread Thread { get; set; }
        [BindProperty]
        public ThreadImages ThreadImages { get; set; }
        public IActionResult OnPost()
        {
            var listMedia = Request.Form["listMedia"];
            foreach(var media in listMedia)
            {
                //listMedia
            }

            Thread.AuthorId = 1;
            Thread.React = 0;
            Thread.Share = 0;
            Thread.SubmitDate = DateTime.Now;
            context.Thread.Add(Thread);


            return Page();
        }
        public void OnGet()
        {
            //var username = User.Identity;
            //var isAuthenticate = User.Identity?.IsAuthenticated ?? false;
            //var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            //return new JsonResult(new { username, isAuthenticate, email });
        }
    }
}
