using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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


            List<Account> accounts = _context.Accounts.Include(x => x.Info).ToList();
           

            accInfo accInfo = new accInfo
            {
                Name = Account.Info.Name,
                UserName = Account.Info.userName,
                Dob = Account.Info.Dob,
                Email = Account.Email,
                Password = Account.Password

            };

            if (confirmPassword != accInfo.Password)
            {

                ModelState.AddModelError("", "Confirm password was wrong.");
                return Page();
            }

            

            foreach (var account in accounts)
            {
                if (accInfo.Email == account.Email)
                {
                    ModelState.AddModelError("", "This email already exists in the system.");
                    return Page();
                }
                if (accInfo.UserName == account.Info?.userName)
                {
                    ModelState.AddModelError("", "This userName already exists in the system.");
                    return Page();
                }

            }

            Account acc = new Account
            {
                Email = accInfo.Email,
                Password = accInfo.Password,
                isActive = true,
                Status = true
            };

            if (acc == null) {
                return Page();
            }
            _context.Accounts.Add(acc);
            _context.SaveChanges();

            Info info = new Info
            {
                UserID = acc.UserID,
                Dob = accInfo.Dob,
                Account = acc,
                Image = String.Empty,
                Name = accInfo.Name,
                Story = "",
                userName = accInfo.UserName,

            };
            if (info == null)
            {
                return Page();

            }
            _context.Info.Add(info);
            _context.SaveChanges();
            return RedirectToPage("/Authentication/Login");
        }

        class accInfo
        {
            public string UserName;

            public string Name;

            public string Email;

            public string Password;
            public DateTime Dob;

        }


    }

}



