using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PRN221_Assignment.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
namespace PRN221_Assignment.Pages.Notification
{
    public class IndexModel : PageModel
    {
        public readonly PRN221_Assignment.Respository.DataContext context;
        public IndexModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
            NoficationShows = new List<NoficationShow>();
        }
        public List<NoficationShow> NoficationShows { get; set; }
        public void OnGet()
        {
            int userId = 0;
            if (User != null && User.Claims != null)
            {
                userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);

            }

          var   NoficationsList = context.UserNofication.Include(x => x.User).Include(x => x.Nofication).Where(x => x.UserId == userId).ToList();
         
            if (NoficationsList.Count > 0 && NoficationsList != null)
            {
                foreach (var Nofication in NoficationsList)
                {
                    NoficationShow item = new NoficationShow();
                    item.Nofication = Nofication.Nofication;
                    item.account.accountt = context.Accounts.Include(x => x.Info).Where(x => x.UserID == Nofication.Nofication.authorId).FirstOrDefault();
                    if(Nofication.Nofication.typeID == 1 || Nofication.Nofication.typeID == 3)
                    {
                        item.thread = context.Thread.Where(x => x.ThreadId == Nofication.Nofication.dataId).FirstOrDefault();
                    }
                    NoficationShows.Add(item);

                   var follower = context.Follow.Where(x => x.UserID == item.account.accountt.UserID && x.FollowerId == userId ).FirstOrDefault();
                    if(follower != null)
                    {
                        item.account.follow = true;
                    }
                    else
                    {
                        item.account.follow = false;
                    }
                    item.account.totalFollower = context.Follow.Where(x => x.UserID == item.account.accountt.UserID).ToList().Count;
                }
                NoficationShows = NoficationShows.OrderByDescending(x => x.Nofication.createdBy).ToList();
            }
           

        }
       public  class NoficationShow
        {
            public Nofication Nofication;
            public PRN221_Assignment.Models.Thread? thread;
            public newAccount account;

            public NoficationShow()
            {
                this.account = new newAccount();
                this.thread = new Models.Thread();
                this.Nofication = new Nofication();
            }
        }
        public class newAccount
        {
            public Account? accountt;
            public bool follow;
            public int totalFollower;
        }
    }
}
