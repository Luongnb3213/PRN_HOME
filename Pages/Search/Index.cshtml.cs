using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using static PRN221_Assignment.Pages.mess.IndexModel;
using PRN221_Assignment.Models;
using static PRN221_Assignment.Pages.Notification.IndexModel;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Authorization;
namespace PRN221_Assignment.Pages.Search
{
    [customAuthorize]

    public class IndexModel : PageModel
    {
        public readonly PRN221_Assignment.Respository.DataContext context;
        public IndexModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
            Accounts = new List<newAccount>();
        }
        public List<newAccount> Accounts { get; set; }
         public class newAccount
        {
            public Account account { get; set; }
            public Info? info { get; set; }
            public int countUser { get; set; }
        } 
        public void OnGet()
        {
            int userId = 0;
            if (User != null && User.Claims != null)
            {
                userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);

            }
            List<Account> Users = context.Accounts.Include(x => x.Info).Where(x => x.UserID != userId).ToList();
           foreach(var user in Users)
            {
                newAccount newaccount = new newAccount();
                newaccount.account = user;
                newaccount.countUser = context.Follow.Where(x => x.UserID == user.UserID).ToList().Count;
                Accounts.Add(newaccount);
            }

        }
        public async Task<IActionResult> OnPostSearchValue([FromBody] string data)
        {

            int userId = 0;
            if (User != null && User.Claims != null)
            {
                userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);

            }
            List<Info> Users = new List<Info>();
            if (!string.IsNullOrEmpty(data))
            {
                Users = context.Info.Where(x => x.UserID != userId && x.userName.Contains(data)).ToList();
            }
            else
            {
                Users = context.Info.Where(x => x.UserID != userId ).ToList();
            }
           
            List<newAccount> AccountSearch = new List<newAccount>();
            foreach (var user in Users)
            {
                newAccount newaccount = new newAccount();
                newaccount.info = user;
                newaccount.countUser = context.Follow.Where(x => x.UserID == user.UserID).ToList().Count;
                AccountSearch.Add(newaccount);
            }

            return new JsonResult(new { data = AccountSearch,text = data });
        }
    }
}
