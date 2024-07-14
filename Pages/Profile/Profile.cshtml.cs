using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRN221_Assignment.Models;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using PRN221_Assignment.Hubs;
using PRN221_Assignment.Authorization;

namespace PRN221_Assignment.Pages.Profile
{
    [customAuthorize]

    public class ProfileModel : PageModel
    {
        private readonly PRN221_Assignment.Respository.DataContext context;
        private readonly IHubContext<chatHub> hubContext;
        public ProfileModel(PRN221_Assignment.Respository.DataContext _context, IHubContext<chatHub> _hubContext)
        {
            context = _context;
            hubContext = _hubContext;
            dicThreadComment = new Dictionary<string, int>();
            dicReact = new Dictionary<string, bool>();
        }
        public Dictionary<string, int> dicThreadComment { get; set; }
        public Account selectedAccount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public List<Models.Thread> myThreads { get; set; }

        [BindProperty]
        public Info info { get; set; }
        public Dictionary<string, bool> dicReact { get; set; }
        public int numFollower { get; set; }
        public int myCurrentId { get; set; }
        public bool isFollow { get; set; }
        [BindProperty(SupportsGet = true)]
        public int partnerId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string type { get; set; }
        public void OnGet()
        {
            if (User != null && User.Claims != null)
            {
                myCurrentId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);
            }

            selectedAccount = context.Accounts.Include(x => x.Info).FirstOrDefault(x => x.UserID == userId);

            myThreads = context.Thread
                .Include(x => x.ThreadReacts)
                .Include(x => x.ThreadImages)
                .Where(x => x.AuthorId == userId).OrderByDescending(x => x.SubmitDate).ToList();


            foreach (var th in myThreads)
            {
                int totalOriginal = context.ThreadComment.Count(x => x.ThreadId == th.ThreadId);
                int totalReply = context.Conversation
                            .Count(x => context.ThreadComment.Where(x => x.ThreadId == th.ThreadId).Select(x => x.ThreadCommentId)
                            .Contains(x.ThreadCommentId));

                int countComment = totalOriginal + totalReply;
                dicThreadComment[th.ThreadId.ToString()] = countComment;
            }

            foreach (var react in myThreads)
            {
                if (react.ThreadReacts.Where(x => x.UserID == userId).Count() != 0)
                {
                    dicReact[react.ThreadId.ToString()] = true;
                }
                else
                {
                    dicReact[react.ThreadId.ToString()] = false;
                }
            }

            List<Follow> myFollower = context.Follow.Include(x => x.Account).Include(x => x.Account.Info).Where(x => x.UserID == userId).ToList();
            numFollower = myFollower.Count();

            var meAndThey = context.Follow.Where(x => x.UserID == userId && x.UserFollowErId == myCurrentId).FirstOrDefault();
            if (meAndThey != null)
            {
                isFollow = true;
            } else
            {
                isFollow = false;
            }

        }
        public class newAccount
        {
            public Info? accountInfo;
            public bool follow;
            public int totalFollower;
        }
        public async Task<IActionResult> OnPostDoRelation()
        {
            var userIdMe = string.Empty;
            if (User != null && User.Claims != null)
            {
                userIdMe = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }

            if (type.Equals("follow"))
            {
                try
                {
                    Follow newFollow = new Follow();
                    newFollow.UserID = partnerId;
                    newFollow.UserFollowErId = Int32.Parse(userIdMe);
                    context.Follow.Add(newFollow);
                    context.SaveChanges();


                    Nofication nofi = new Nofication();
                    nofi.createdBy = DateTime.Now;
                    nofi.authorId = Int32.Parse(userIdMe);
                    nofi.typeID = 2;
                    nofi.dataId = Int32.Parse(userIdMe);
                    context.Nofication.Add(nofi);
                    context.SaveChanges();

                    UserNofication userNofication = new UserNofication();
                    userNofication.NoficationId = nofi.Id;
                    userNofication.UserId = partnerId;
                    context.UserNofication.Add(userNofication);
                    context.SaveChanges();
                    var account = context.Info.Where(x => x.UserID == Int32.Parse(userIdMe)).FirstOrDefault();
                    var isFollow = context.Follow.Where(x => x.UserFollowErId == partnerId && x.UserID == Int32.Parse(userIdMe)).FirstOrDefault();
                    bool isFollower = false;
                    if (isFollow != null)
                    {
                        isFollower = true;
                    }
                    await hubContext.Clients.User(partnerId + "").SendAsync("receiveNofication", userIdMe, nofi.typeID, account, isFollower, context.Follow.Where(x => x.UserFollowErId == Int32.Parse(userIdMe)).ToList().Count);

                }
                catch(Exception e) {
                }
               
            } else if (type.Equals("unfollow"))
            {
                Follow deleteFollow = context.Follow.Where(x => x.UserID == partnerId && x.UserFollowErId == Int32.Parse(userIdMe)).FirstOrDefault();
                context.Follow.Remove(deleteFollow);
                context.SaveChanges();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return new JsonResult(new { data = userIdMe }, options);
        }
        public IActionResult OnPost()
        {
            selectedAccount = context.Accounts.Include(x => x.Info).FirstOrDefault(x => x.UserID == userId);
            selectedAccount.Info.Name = info.Name;
            selectedAccount.Info.userName = info.userName;
            selectedAccount.Info.Story = info.Story;         
            context.SaveChanges();
            return RedirectToPage("profile", new { userId = selectedAccount.UserID });
        }  
    }
}
