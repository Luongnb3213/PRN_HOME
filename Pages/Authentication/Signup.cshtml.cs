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
            Account acc = new Account {
                Email = Account.Email,
                Password = Account.Password,
                Status = true,
            isActive = true,
            };

            
            
            
            string confirmPassword = Request.Form["confirmPassword"];
            if (acc != null)
            {
                if (confirmPassword != acc.Password)
                {
                    return Page();
                }

                //foreach (var account in listAcc)
                //{
                //    if (acc.Email == account.Email)
                //    {
                //        return Page();
                //    }
                //}



               

                var demo = _context.Accounts.Add(acc);
                
                
                _context.SaveChanges();

                Info info = new Info
                {

                    userName = Account.Info.userName,
                    Name = Account.Info.Name,
                    Story = String.Empty,
                    Dob = Account.Info.Dob,
                    Image = String.Empty,

                };


                _context.Info.Add(info);
                _context.SaveChanges();
                return RedirectToPage("/Authentication/Login");
            }



            return Page();

        }
    }

}



