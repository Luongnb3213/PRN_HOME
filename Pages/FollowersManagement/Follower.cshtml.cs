using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Authorization;
using PRN221_Assignment.Hubs;
using PRN221_Assignment.Models;
using System.Security.Claims;

namespace PRN221_Assignment.Pages.FollowersManagement
{
    [customAuthorize]

    public class IndexModel : PageModel
    {
        public readonly PRN221_Assignment.Respository.DataContext context;
      
        public IndexModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
            myFollowerCorrect = new Dictionary<int, bool>();
        }
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public List<Account> myFollower { get; set; }
        public Dictionary<int, bool> myFollowerCorrect { get; set; }
        public int myCurrentId { get; set; }

        public void OnGet()
        {

            OnGetGetListFollow();
        }
        public void OnGetGetListFollow()
        {
            var userIdMe = string.Empty;
            if (User != null && User.Claims != null)
            {
                userIdMe = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            myCurrentId = Int32.Parse(userIdMe);
            List<int> myFollowerId = context.Follow.Where(x => x.UserID == userId).Select(x => x.UserFollowErId).ToList();
            myFollower = context.Accounts.Include(x => x.Info).Where(x => myFollowerId.Contains(x.UserID)).ToList();
            var checkFollower = context.Follow.Where(x => myFollowerId.Contains(x.UserID) && x.UserFollowErId == Int32.Parse(userIdMe)).Select(x => x.UserID).ToList();
            foreach (var account in myFollower)
            {
                myFollowerCorrect[account.UserID] = checkFollower.Contains(account.UserID) ? true : false;
            }
        }
    }
}
