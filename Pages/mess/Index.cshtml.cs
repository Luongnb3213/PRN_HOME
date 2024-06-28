using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using static PRN221_Assignment.Pages.mess.IndexModel;
using Microsoft.AspNetCore.SignalR;
using PRN221_Assignment.Hubs;

namespace PRN221_Assignment.Pages.mess
{
    public class IndexModel : PageModel
    {
        public readonly PRN221_Assignment.Respository.DataContext context;
        private readonly IHubContext<chatHub> hubContext;

        public IndexModel(PRN221_Assignment.Respository.DataContext _context, IHubContext<chatHub> _hubContext)
        {
            context = _context;
            hubContext = _hubContext;
            dicFollower = new Dictionary<int, Account>();
            dicFollowerMesage = new Dictionary<int, dynamic>();
            listFollowerDown = new List<dynamic>();
        }
        public Dictionary<int, Account> dicFollower { get; set; }
        public Dictionary<int, dynamic> dicFollowerMesage { get; set; }
        public List<Account> listFollower { get; set; }
        public List<dynamic> ChatBox { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PartnerId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool selected { get; set; }
        public int currentUserId { get; set; }
        public List<dynamic> listFollowerDown { get; set; }
        public void OnGet()
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            currentUserId = Int32.Parse(userId);
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

            var listMsgExistWithId = (from a in context.Mess
                                      join b in context.MessageReceive on a.messId equals b.messID
                                      select new
                                      {
                                          a.AuthorId,
                                          ReceiveId = b.UserId,
                                      }).Where(x => (x.AuthorId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.ReceiveId)) || (x.ReceiveId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.AuthorId))).ToList();

            List<int> acceptableFollowerFull = new List<int>();

            foreach (var id in listMsgExistWithId)
            {
                acceptableFollowerFull.Add(id.AuthorId);
                acceptableFollowerFull.Add(id.ReceiveId);
            }

            var acceptableFollower = acceptableFollowerFull.Distinct();

            List<ChatHistory> allChat = (from a in context.Mess
                                         join b in context.MessageReceive on a.messId equals b.messID
                                         join c in context.Accounts on a.AuthorId equals c.UserID
                                         join d in context.Accounts on b.UserId equals d.UserID
                                         join e in context.Info on a.AuthorId equals e.UserID
                                         join f in context.Info on b.UserId equals f.UserID
                                         select new ChatHistory()
                                         {
                                             AuthorId = a.AuthorId,
                                             Content = a.Content,
                                             createdBy = a.createdBy,
                                             ReceiveId = b.UserId,
                                             whose = (a.AuthorId == Int32.Parse(userId) ? "me" : "other"),
                                             avtAuthor = e.Image,
                                             avtPartner = f.Image,
                                             PartnerUsername = f.userName,
                                             MyUsername = e.userName,
                                         }).Where(x => (x.AuthorId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.ReceiveId)) || (x.ReceiveId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.AuthorId)))
                           .OrderByDescending(x => x.createdBy)
                           .ToList();

            var listDup = new List<ChatHistory>();
            foreach (ChatHistory a in allChat)
            {
                listDup.Add(new ChatHistory()
                {
                    AuthorId = a.ReceiveId,
                    Content = a.Content,
                    createdBy = a.createdBy,
                    ReceiveId = a.AuthorId,
                    whose = a.whose,
                    avtAuthor = a.avtPartner,
                    avtPartner = a.avtAuthor,
                    PartnerUsername = a.PartnerUsername,
                    MyUsername = a.MyUsername
                });
            }
            allChat.AddRange(listDup);

            var chatGroupMin = allChat.GroupBy(x => new { x.ReceiveId, x.AuthorId }).Select(g => new ChatHistory()
            {
                AuthorId = g.Key.AuthorId,
                Content = g.MaxBy(x => x.createdBy).Content,
                createdBy = g.MaxBy(x => x.createdBy).createdBy,
                ReceiveId = g.MaxBy(x => x.createdBy).ReceiveId,
                whose = g.MaxBy(x => x.createdBy).whose,
                avtAuthor = g.MaxBy(x => x.createdBy).avtAuthor,
                avtPartner = g.MaxBy(x => x.createdBy).avtPartner,
                PartnerUsername = g.MaxBy(x => x.createdBy).PartnerUsername,
                MyUsername = g.MaxBy(x => x.createdBy).MyUsername
            });

            foreach (var chat in chatGroupMin)
            {
                if ((chat.whose.Equals("me") && chat.AuthorId == Int32.Parse(userId)) || (chat.whose.Equals("other") && chat.ReceiveId == Int32.Parse(userId)))
                {
                    listFollowerDown.Add(chat);
                }
            }

            //listFollowerDown = context.Accounts
            //    .Include(x => x.Mess)
            //    .Include(x => x.Info)
            //    .Where(x => acceptableFollower.Contains(x.UserID) && x.UserID != Int32.Parse(userId))
            //    .ToList();

            //foreach (var acc in listFollowerDown)
            //{
            //    var mess = (from a in context.Mess
            //                join b in context.MessageReceive on a.messId equals b.messID
            //                select new
            //                {
            //                    a.AuthorId,
            //                    a.Content,
            //                    ReceiveId = b.UserId,
            //                    a.createdBy,
            //                    whose = (a.AuthorId == Int32.Parse(userId) ? "You: " : "")
            //                }).Where(x => (x.AuthorId == Int32.Parse(userId) && x.ReceiveId == acc.UserID) || (x.ReceiveId == Int32.Parse(userId) && x.AuthorId == acc.UserID)).OrderByDescending(x => x.createdBy).First();
            //    dicFollowerMesage[acc.UserID] = mess;
            //}
        }
        public IActionResult OnGetGetBoxChat()
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
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
        public async Task<IActionResult> OnPost([FromBody] MessageModel data)
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            Mess newMess = new Mess();
            newMess.Content = data.messContent;
            newMess.AuthorId = Int32.Parse(userId);
            newMess.createdBy = DateTime.Now;
            newMess.type = false;
            context.Mess.Add(newMess);
            context.SaveChanges();

            MessageReceive newMessageReceive = new MessageReceive();
            newMessageReceive.messID = newMess.messId;
            newMessageReceive.GroupID = 1;
            newMessageReceive.UserId = data.partnerId;
            newMessageReceive.seen = false;
            newMessageReceive.type = false;
            context.MessageReceive.Add(newMessageReceive);
            context.SaveChanges();

            var listMess = (from a in context.Mess
                            join b in context.MessageReceive on a.messId equals b.messID
                            join c in context.Accounts on a.AuthorId equals c.UserID
                            join d in context.Accounts on b.UserId equals d.UserID
                            join e in context.Info on a.AuthorId equals e.UserID
                            join f in context.Info on b.UserId equals f.UserID
                            select new
                            {
                                a.messId,
                                a.Content,
                                a.AuthorId,
                                a.createdBy,
                                AuthorUsername = c.Info.userName,
                                TypeNoti = a.type,
                                b.MessageReceiveId,
                                b.GroupID,
                                ReceivePerson = b.UserId,
                                ReceiveName = d.Info.userName,
                                b.seen,
                                TypeMess = b.type,
                                whose = (a.AuthorId == Int32.Parse(userId) ? "me" : "other"),
                                avtAuthor = e.Image,
                                avtPartner = f.Image
                            }).Where(x => x.AuthorId == Int32.Parse(userId) && x.ReceivePerson == data.partnerId).ToList();

            var latestRecord = listMess.OrderByDescending(x => x.createdBy).FirstOrDefault();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return new JsonResult(new { data = latestRecord }, options);
        }
        public class MessageModel
        {
            public string messContent { get; set; }
            public int partnerId { get; set; }
        }
        public class ChatHistory
        {
            public int AuthorId { get; set; }
            public string Content { get; set; }
            public DateTime createdBy { get; set; }
            public int ReceiveId { get; set; }
            public string whose { get; set; }
            public string avtAuthor { get; set; }
            public string avtPartner { get; set; }
            public string PartnerUsername { get; set; }
            public string MyUsername { get; set; }
        }
    }
}
