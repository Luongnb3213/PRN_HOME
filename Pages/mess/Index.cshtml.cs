using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using System.Security.Claims;

namespace PRN221_Assignment.Pages.mess
{
    public class IndexModel : PageModel
    {
        public readonly PRN221_Assignment.Respository.DataContext context;
        public IndexModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
            dicFollower = new Dictionary<int, Account>();
        }
        public Dictionary<int, Account> dicFollower { get; set; }
        public List<Account> listFollower { get; set; }
        public void OnGet()
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            var listFollowerId = context.Follow
                .Include(x => x.Account)
                .Include(x => x.Account.Info)
                .Where(x => x.UserID == Int32.Parse(userId)).Select(x => x.UserFollowErId).ToList();
            var listFollowerCorrect = context.Follow
                .Include(x => x.Account)
                .Include(x => x.Account.Info)
                .Where(x => listFollowerId.Contains(x.UserID) && x.UserFollowErId == Int32.Parse(userId)).Select(x => x.UserID).ToList();
            listFollower = context.Accounts.Include(x => x.Info).Where(x => listFollowerCorrect.Contains(x.UserID)).ToList();
            foreach (var account in listFollower)
            {
                dicFollower[account.UserID] = account;
            }
        }
    }
}
