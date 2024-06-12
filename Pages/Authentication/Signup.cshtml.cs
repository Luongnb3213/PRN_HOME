using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Models;
namespace PRN221_Assignment.Pages.Authentication
{
    public class SignupModel : PageModel
    {

        private readonly PRN221_Assignment.Respository.DataContext _context;

        public SignupModel(PRN221_Assignment.Respository.DataContext context)
        {
            _context = context;

        }


        public void OnGet()
        {
        }


        [BindProperty]
        public Account? Account { get; set; }

        public IActionResult OnPost()
        {
            string confirmPassword = Request.Form["confirmPassword"];
            if (Account != null)
            {
                if (confirmPassword != Account.Password)
                {
                    return Page();
                }
                _context.Accounts.Add(Account);
                _context.SaveChanges();
                return RedirectToAction("/Authentication/Login");
            }



            return Page();

        }
    }

}



