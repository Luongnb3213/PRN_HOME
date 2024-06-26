using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using static PRN221_Assignment.Pages.mess.IndexModel;

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
        public List<dynamic> ChatBox { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PartnerId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool selected { get; set; }
        public IActionResult OnGet()
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

            if (selected)
            {
                List<int> peopleInChat = new List<int>();
                peopleInChat.Add(PartnerId);
                peopleInChat.Add(Int32.Parse(userId));
                ChatBox = (from a in context.Mess
                           join b in context.MessageReceive on a.messId equals b.messID
                           join c in context.Accounts on a.AuthorId equals c.UserID
                           join d in context.Accounts on b.UserId equals d.UserID
                           join e in context.Info on a.AuthorId equals e.UserID
                           select new
                           {
                               a.messId,
                               a.Content,
                               a.AuthorId,
                               a.createdBy,
                               TypeNoti = a.type,
                               b.MessageReceiveId,
                               b.GroupID,
                               ReceivePerson = b.UserId,
                               b.seen,
                               TypeMess = b.type,
                               whose = (a.AuthorId == Int32.Parse(userId) ? "me" : "other"),
                               avtAuthor = e.Image,
                           }).Where(x => peopleInChat.Contains(x.AuthorId) && peopleInChat.Contains(x.ReceivePerson)).Cast<dynamic>().ToList();
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                return new JsonResult(new { data = ChatBox }, options);
            }
            return Page();
        }

        public IActionResult OnPost(int partnerId)
        {
            return new JsonResult(new { success = true });
        }
        public class MessageModel
        {
            public string MessContent { get; set; }
            public int PartnerId { get; set; }
        }
    }
}
