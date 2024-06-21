using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRN221_Assignment.Models;
using System.Security.Claims;

namespace PRN221_Assignment.Pages.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly PRN221_Assignment.Respository.DataContext context;
        public ProfileModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
        }
        public Account selectedAccount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public List<Models.Thread> myThreads { get; set; }
        public void OnGet()
        {
            selectedAccount = context.Accounts.Include(x => x.Info).FirstOrDefault(x => x.UserID == userId);

            myThreads = context.Thread.Include(x => x.ThreadImages).Where(x => x.AuthorId == userId).OrderByDescending(x => x.SubmitDate).ToList();
        }
    }
}
