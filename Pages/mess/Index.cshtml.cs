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
using System.Text.RegularExpressions;

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
        public List<dynamic> GroupChatBoxAll { get; set; }
        [BindProperty(SupportsGet = true)]
        public int groupId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PartnerId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool selected { get; set; }
        public int currentUserId { get; set; }
        public List<dynamic> listFollowerDown { get; set; }
        public List<Account> listFollowerToGroup { get; set; }
        [BindProperty]
        public List<string> SelectedUsers { get; set; }
        [BindProperty]
        public string NameGroup { get; set; }
        public void OnGet()
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            currentUserId = Int32.Parse(userId);
            //my follower
            var listFollowerId = context.Follow
                .Include(x => x.Account)
                .Include(x => x.Account.Info)
                .Where(x => x.UserID == Int32.Parse(userId)).Select(x => x.UserFollowErId).ToList();

            //Follower 2 sides
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
                                             AuthorUsername = e.userName,
                                             PartnerUsername = f.userName,
                                             type = b.type
                                         }).Where(x => ((x.AuthorId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.ReceiveId)) || (x.ReceiveId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.AuthorId))) && x.type == false)
                           .OrderByDescending(x => x.createdBy)
                           .ToList();

            var listMyGroup = context.GroupUser.Where(x => x.UserId == Int32.Parse(userId)).Select(x => x.GroupId).ToList();


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
                    avtAuthor = a.avtAuthor,
                    avtPartner = a.avtPartner,
                    AuthorUsername = a.AuthorUsername,
                    PartnerUsername = a.PartnerUsername,
                    type = a.type
                });
            }
            allChat.AddRange(listDup);

            List<ChatHistory> allChatGroup = (from a in context.Mess
                                              join b in context.MessageReceive on a.messId equals b.messID
                                              join c in context.Accounts on a.AuthorId equals c.UserID
                                              join d in context.Accounts on b.UserId equals d.UserID
                                              join e in context.Info on a.AuthorId equals e.UserID
                                              join f in context.Info on b.UserId equals f.UserID
                                              join p in context.Group on b.GroupID equals p.GroupId
                                              select new ChatHistory()
                                              {
                                                  AuthorId = a.AuthorId,
                                                  Content = a.Content,
                                                  createdBy = a.createdBy,
                                                  ReceiveId = b.UserId,
                                                  whose = (a.AuthorId == Int32.Parse(userId) ? "me" : "other"),
                                                  avtAuthor = e.Image,
                                                  avtPartner = p.Image,
                                                  AuthorUsername = e.userName,
                                                  PartnerUsername = p.Name,
                                                  type = b.type,
                                                  GroupId = b.GroupID
                                              }).Where(x => listMyGroup.Contains(x.GroupId) && x.type == true)
                           .OrderByDescending(x => x.createdBy)
                           .ToList();
            //false la single, true la group

            var chatGroupOfGroup = allChatGroup.GroupBy(x => new { x.GroupId }).Select(g =>
            {
                var maxByCreated = g.MaxBy(x => x.createdBy);
                return new ChatHistory()
                {
                    AuthorId = maxByCreated.AuthorId,
                    Content = maxByCreated.Content,
                    createdBy = maxByCreated.createdBy,
                    ReceiveId = maxByCreated.ReceiveId,
                    whose = maxByCreated.whose,
                    avtAuthor = maxByCreated.avtAuthor,
                    avtPartner = maxByCreated.avtPartner,
                    AuthorUsername = maxByCreated.AuthorUsername,
                    PartnerUsername = maxByCreated.PartnerUsername,
                    displayAvt = maxByCreated.avtPartner,
                    displayUsername = maxByCreated.PartnerUsername,
                    IdToClick = maxByCreated.GroupId,
                    GroupId = maxByCreated.GroupId,
                    type = maxByCreated.type
                };
            }).ToList();

            //var chatGroupMin = allChat.GroupBy(x => new { x.ReceiveId, x.AuthorId }).Select(g => new ChatHistory()
            //{
            //    AuthorId = g.Key.AuthorId,
            //    Content = g.MaxBy(x => x.createdBy).Content,
            //    createdBy = g.MaxBy(x => x.createdBy).createdBy,
            //    ReceiveId = g.MaxBy(x => x.createdBy).ReceiveId,
            //    whose = g.MaxBy(x => x.createdBy).whose,
            //    avtAuthor = g.MaxBy(x => x.createdBy).avtAuthor,
            //    avtPartner = g.MaxBy(x => x.createdBy).avtPartner,
            //    AuthorUsername = g.MaxBy(x => x.createdBy).AuthorUsername,
            //    PartnerUsername = g.MaxBy(x => x.createdBy).PartnerUsername,
            //    displayAvt = whose.Equals()
            //});

            var chatGroupMin = allChat.GroupBy(x => new { x.ReceiveId, x.AuthorId }).Select(g =>
            {
                var maxByCreated = g.MaxBy(x => x.createdBy);
                return new ChatHistory()
                {
                    AuthorId = g.Key.AuthorId,
                    Content = maxByCreated.Content,
                    createdBy = maxByCreated.createdBy,
                    ReceiveId = maxByCreated.ReceiveId,
                    whose = maxByCreated.whose,
                    avtAuthor = maxByCreated.avtAuthor,
                    avtPartner = maxByCreated.avtPartner,
                    AuthorUsername = maxByCreated.AuthorUsername,
                    PartnerUsername = maxByCreated.PartnerUsername,
                    displayAvt = maxByCreated.whose.Equals("other") ? maxByCreated.avtAuthor : maxByCreated.avtPartner,
                    displayUsername = maxByCreated.whose.Equals("other") ? maxByCreated.AuthorUsername : maxByCreated.PartnerUsername,
                    IdToClick = maxByCreated.whose.Equals("other") ? maxByCreated.AuthorId : maxByCreated.ReceiveId,
                    type = maxByCreated.type
                };
            }).ToList();

            if (chatGroupMin.Count != 0)
            {
                foreach (var chat in chatGroupMin)
                {
                    if ((chat.whose.Equals("me") && chat.AuthorId == Int32.Parse(userId)) || (chat.whose.Equals("other") && chat.ReceiveId == Int32.Parse(userId)))
                    {
                        listFollowerDown.Add(chat);
                    }
                }
            }

            if (chatGroupOfGroup.Count != 0)
            {
                foreach (var chat in chatGroupOfGroup)
                {
                    listFollowerDown.Add(chat);
                }
            }

            listFollowerDown = listFollowerDown.OrderByDescending(x => x.createdBy).ToList();

            //Follower to create group
            listFollowerToGroup = context.Accounts.Include(x => x.Info).Where(x => listFollowerCorrect.Contains(x.UserID)).ToList();
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
                       }).Where(x => peopleInChat.Contains(x.AuthorId) && peopleInChat.Contains(x.ReceivePerson) && x.TypeMess == false).Cast<dynamic>().ToList();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return new JsonResult(new { data = ChatBox }, options);
        }
        public IActionResult OnGetGetGroupChat()
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            int numMembers = context.GroupUser.Count(x => x.GroupId == groupId);

            GroupChatBoxAll = (from a in context.Mess
                               join b in context.MessageReceive on a.messId equals b.messID
                               join c in context.Accounts on a.AuthorId equals c.UserID
                               join d in context.Accounts on b.UserId equals d.UserID
                               join e in context.Info on a.AuthorId equals e.UserID
                               join f in context.Info on b.UserId equals f.UserID
                               join g in context.Group on b.GroupID equals g.GroupId
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
                               }).Where(x => x.TypeMess == true && x.GroupID == groupId).Cast<dynamic>().ToList();

            var groupedGroupChat = GroupChatBoxAll.GroupBy(x => new { x.AuthorId, x.Content }).Select(g =>
            {
                var first = g.First();
                return new
                {
                    first.messId,
                    first.Content,
                    first.AuthorId,
                    first.createdBy,
                    first.TypeNoti,
                    first.MessageReceiveId,
                    first.GroupID,
                    first.ReceivePerson,
                    first.seen,
                    first.TypeMess,
                    first.whose,
                    first.avtAuthor,
                };
            }).ToList();

            string nameGroup = context.Group.Where(x => x.GroupId == groupId).Select(x => x.Name).FirstOrDefault();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return new JsonResult(new { data = groupedGroupChat, num = numMembers, nameGroup = nameGroup }, options);
        }
        public IActionResult OnPostCreateGroup()
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            Models.Group newGroup = new Models.Group();
            newGroup.Name = NameGroup;
            newGroup.Image = "default/group.png";
            newGroup.createdBy = DateTime.Now;
            context.Group.Add(newGroup);
            context.SaveChanges();

            foreach (var u in SelectedUsers)
            {
                GroupUser newGroupUser = new GroupUser();
                newGroupUser.UserId = Int32.Parse(u);
                newGroupUser.GroupId = newGroup.GroupId;
                context.GroupUser.Add(newGroupUser);
                context.SaveChanges();
            }
            GroupUser meInGroup = new GroupUser();
            meInGroup.UserId = Int32.Parse(userId);
            meInGroup.GroupId = newGroup.GroupId;
            context.GroupUser.Add(meInGroup);
            context.SaveChanges();

            return RedirectToPage("./Index");
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
        public IActionResult OnPostGroup([FromBody] MessageModelGroup dataGroup)
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            var other = context.GroupUser.Where(x => x.GroupId == dataGroup.groupId && x.UserId != Int32.Parse(userId)).Select(x => x.UserId).ToList();

            Mess newMess = new Mess();
            newMess.Content = dataGroup.messContent;
            newMess.AuthorId = Int32.Parse(userId);
            newMess.createdBy = DateTime.Now;
            newMess.type = false;
            context.Mess.Add(newMess);
            context.SaveChanges();

            foreach (var o in other)
            {
                MessageReceive newReceive = new MessageReceive();
                newReceive.messID = newMess.messId;
                newReceive.GroupID = dataGroup.groupId;
                newReceive.UserId = o;
                newReceive.seen = false;
                newReceive.type = true;
                context.MessageReceive.Add(newReceive);
                context.SaveChanges();
            }

            var listMess = (from a in context.Mess
                            join b in context.MessageReceive on a.messId equals b.messID
                            join c in context.Accounts on a.AuthorId equals c.UserID
                            join d in context.Accounts on b.UserId equals d.UserID
                            join e in context.Info on a.AuthorId equals e.UserID
                            join f in context.Info on b.UserId equals f.UserID
                            join g in context.Group on b.GroupID equals g.GroupId
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
                                avtPartner = f.Image,
                                avtGroup = g.Image,
                                groupName = g.Name,
                                groupImg = g.Image
                            }).Where(x => x.AuthorId == Int32.Parse(userId) && x.GroupID == dataGroup.groupId).ToList();

            var latestRecord = listMess.OrderByDescending(x => x.createdBy).FirstOrDefault();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return new JsonResult(new { data = latestRecord }, options);
        }
        public IActionResult OnGetGetFlexibleChatBar()
        {
            //var userId = string.Empty;
            //if (User != null && User.Claims != null)
            //{
            //    userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            //}
            //currentUserId = Int32.Parse(userId);
            ////my follower
            //var listFollowerId = context.Follow
            //    .Include(x => x.Account)
            //    .Include(x => x.Account.Info)
            //    .Where(x => x.UserID == Int32.Parse(userId)).Select(x => x.UserFollowErId).ToList();
            //var listFollowerCorrect = context.Follow
            //    .Include(x => x.Account)
            //    .Include(x => x.Account.Info)
            //    .Where(x => listFollowerId.Contains(x.UserID) && x.UserFollowErId == Int32.Parse(userId)).Select(x => x.UserID).ToList();
            //var listMyGroup = context.GroupUser.Where(x => x.UserId == Int32.Parse(userId)).Select(x => x.GroupId).ToList();

            //List<ChatHistory> allChat = (from a in context.Mess
            //                             join b in context.MessageReceive on a.messId equals b.messID
            //                             join c in context.Accounts on a.AuthorId equals c.UserID
            //                             join d in context.Accounts on b.UserId equals d.UserID
            //                             join e in context.Info on a.AuthorId equals e.UserID
            //                             join f in context.Info on b.UserId equals f.UserID
            //                             select new ChatHistory()
            //                             {
            //                                 AuthorId = a.AuthorId,
            //                                 Content = a.Content,
            //                                 createdBy = a.createdBy,
            //                                 ReceiveId = b.UserId,
            //                                 whose = (a.AuthorId == Int32.Parse(userId) ? "me" : "other"),
            //                                 avtAuthor = e.Image,
            //                                 avtPartner = f.Image,
            //                                 AuthorUsername = e.userName,
            //                                 PartnerUsername = f.userName,
            //                                 type = b.type
            //                             }).Where(x => ((x.AuthorId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.ReceiveId)) || (x.ReceiveId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.AuthorId))) && x.type == false)
            //               .OrderByDescending(x => x.createdBy)
            //               .ToList();
            //List<ChatHistory> allChatGroup = (from a in context.Mess
            //                                  join b in context.MessageReceive on a.messId equals b.messID
            //                                  join c in context.Accounts on a.AuthorId equals c.UserID
            //                                  join d in context.Accounts on b.UserId equals d.UserID
            //                                  join e in context.Info on a.AuthorId equals e.UserID
            //                                  join f in context.Info on b.UserId equals f.UserID
            //                                  join p in context.Group on b.GroupID equals p.GroupId
            //                                  select new ChatHistory()
            //                                  {
            //                                      AuthorId = a.AuthorId,
            //                                      Content = a.Content,
            //                                      createdBy = a.createdBy,
            //                                      ReceiveId = b.UserId,
            //                                      whose = (a.AuthorId == Int32.Parse(userId) ? "me" : "other"),
            //                                      avtAuthor = e.Image,
            //                                      avtPartner = p.Image,
            //                                      AuthorUsername = e.userName,
            //                                      PartnerUsername = p.Name,
            //                                      type = b.type,
            //                                      GroupId = b.GroupID
            //                                  }).Where(x => listMyGroup.Contains(x.GroupId) && x.type == true)
            //   .OrderByDescending(x => x.createdBy)
            //   .ToList();
            //var chatGroupOfGroup = allChatGroup.GroupBy(x => new { x.GroupId }).Select(g =>
            //{
            //    var maxByCreated = g.MaxBy(x => x.createdBy);
            //    return new ChatHistory()
            //    {
            //        AuthorId = maxByCreated.AuthorId,
            //        Content = maxByCreated.Content,
            //        createdBy = maxByCreated.createdBy,
            //        ReceiveId = maxByCreated.ReceiveId,
            //        whose = maxByCreated.whose,
            //        avtAuthor = maxByCreated.avtAuthor,
            //        avtPartner = maxByCreated.avtPartner,
            //        AuthorUsername = maxByCreated.AuthorUsername,
            //        PartnerUsername = maxByCreated.PartnerUsername,
            //        displayAvt = maxByCreated.avtPartner,
            //        displayUsername = maxByCreated.PartnerUsername,
            //        IdToClick = maxByCreated.GroupId,
            //        GroupId = maxByCreated.GroupId,
            //        type = maxByCreated.type
            //    };
            //}).ToList();

            //var chatGroupMin = allChat.GroupBy(x => new { x.ReceiveId, x.AuthorId }).Select(g =>
            //{
            //    var maxByCreated = g.MaxBy(x => x.createdBy);
            //    return new ChatHistory()
            //    {
            //        AuthorId = g.Key.AuthorId,
            //        Content = maxByCreated.Content,
            //        createdBy = maxByCreated.createdBy,
            //        ReceiveId = maxByCreated.ReceiveId,
            //        whose = maxByCreated.whose,
            //        avtAuthor = maxByCreated.avtAuthor,
            //        avtPartner = maxByCreated.avtPartner,
            //        AuthorUsername = maxByCreated.AuthorUsername,
            //        PartnerUsername = maxByCreated.PartnerUsername,
            //        displayAvt = maxByCreated.whose.Equals("other") ? maxByCreated.avtAuthor : maxByCreated.avtPartner,
            //        displayUsername = maxByCreated.whose.Equals("other") ? maxByCreated.AuthorUsername : maxByCreated.PartnerUsername,
            //        IdToClick = maxByCreated.whose.Equals("other") ? maxByCreated.AuthorId : maxByCreated.ReceiveId,
            //        type = maxByCreated.type
            //    };
            //}).ToList();
            //if (chatGroupMin.Count != 0)
            //{
            //    foreach (var chat in chatGroupMin)
            //    {
            //        if ((chat.whose.Equals("me") && chat.AuthorId == Int32.Parse(userId)) || (chat.whose.Equals("other") && chat.ReceiveId == Int32.Parse(userId)))
            //        {
            //            listFollowerDown.Add(chat);
            //        }
            //    }
            //}

            //if (chatGroupOfGroup.Count != 0)
            //{
            //    foreach (var chat in chatGroupOfGroup)
            //    {
            //        listFollowerDown.Add(chat);
            //    }
            //}

            //listFollowerDown = listFollowerDown.OrderByDescending(x => x.createdBy).ToList();
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            currentUserId = Int32.Parse(userId);
            //my follower
            var listFollowerId = context.Follow
                .Include(x => x.Account)
                .Include(x => x.Account.Info)
                .Where(x => x.UserID == Int32.Parse(userId)).Select(x => x.UserFollowErId).ToList();

            //Follower 2 sides
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
                                             AuthorUsername = e.userName,
                                             PartnerUsername = f.userName,
                                             type = b.type
                                         }).Where(x => ((x.AuthorId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.ReceiveId)) || (x.ReceiveId == Int32.Parse(userId) && listFollowerCorrect.Contains(x.AuthorId))) && x.type == false)
                           .OrderByDescending(x => x.createdBy)
                           .ToList();

            var listMyGroup = context.GroupUser.Where(x => x.UserId == Int32.Parse(userId)).Select(x => x.GroupId).ToList();


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
                    avtAuthor = a.avtAuthor,
                    avtPartner = a.avtPartner,
                    AuthorUsername = a.AuthorUsername,
                    PartnerUsername = a.PartnerUsername,
                    type = a.type
                });
            }
            allChat.AddRange(listDup);

            List<ChatHistory> allChatGroup = (from a in context.Mess
                                              join b in context.MessageReceive on a.messId equals b.messID
                                              join c in context.Accounts on a.AuthorId equals c.UserID
                                              join d in context.Accounts on b.UserId equals d.UserID
                                              join e in context.Info on a.AuthorId equals e.UserID
                                              join f in context.Info on b.UserId equals f.UserID
                                              join p in context.Group on b.GroupID equals p.GroupId
                                              select new ChatHistory()
                                              {
                                                  AuthorId = a.AuthorId,
                                                  Content = a.Content,
                                                  createdBy = a.createdBy,
                                                  ReceiveId = b.UserId,
                                                  whose = (a.AuthorId == Int32.Parse(userId) ? "me" : "other"),
                                                  avtAuthor = e.Image,
                                                  avtPartner = p.Image,
                                                  AuthorUsername = e.userName,
                                                  PartnerUsername = p.Name,
                                                  type = b.type,
                                                  GroupId = b.GroupID
                                              }).Where(x => listMyGroup.Contains(x.GroupId) && x.type == true)
                           .OrderByDescending(x => x.createdBy)
                           .ToList();
            //false la single, true la group

            var chatGroupOfGroup = allChatGroup.GroupBy(x => new { x.GroupId }).Select(g =>
            {
                var maxByCreated = g.MaxBy(x => x.createdBy);
                return new ChatHistory()
                {
                    AuthorId = maxByCreated.AuthorId,
                    Content = maxByCreated.Content,
                    createdBy = maxByCreated.createdBy,
                    ReceiveId = maxByCreated.ReceiveId,
                    whose = maxByCreated.whose,
                    avtAuthor = maxByCreated.avtAuthor,
                    avtPartner = maxByCreated.avtPartner,
                    AuthorUsername = maxByCreated.AuthorUsername,
                    PartnerUsername = maxByCreated.PartnerUsername,
                    displayAvt = maxByCreated.avtPartner,
                    displayUsername = maxByCreated.PartnerUsername,
                    IdToClick = maxByCreated.GroupId,
                    GroupId = maxByCreated.GroupId,
                    type = maxByCreated.type
                };
            }).ToList();

            //var chatGroupMin = allChat.GroupBy(x => new { x.ReceiveId, x.AuthorId }).Select(g => new ChatHistory()
            //{
            //    AuthorId = g.Key.AuthorId,
            //    Content = g.MaxBy(x => x.createdBy).Content,
            //    createdBy = g.MaxBy(x => x.createdBy).createdBy,
            //    ReceiveId = g.MaxBy(x => x.createdBy).ReceiveId,
            //    whose = g.MaxBy(x => x.createdBy).whose,
            //    avtAuthor = g.MaxBy(x => x.createdBy).avtAuthor,
            //    avtPartner = g.MaxBy(x => x.createdBy).avtPartner,
            //    AuthorUsername = g.MaxBy(x => x.createdBy).AuthorUsername,
            //    PartnerUsername = g.MaxBy(x => x.createdBy).PartnerUsername,
            //    displayAvt = whose.Equals()
            //});

            var chatGroupMin = allChat.GroupBy(x => new { x.ReceiveId, x.AuthorId }).Select(g =>
            {
                var maxByCreated = g.MaxBy(x => x.createdBy);
                return new ChatHistory()
                {
                    AuthorId = g.Key.AuthorId,
                    Content = maxByCreated.Content,
                    createdBy = maxByCreated.createdBy,
                    ReceiveId = maxByCreated.ReceiveId,
                    whose = maxByCreated.whose,
                    avtAuthor = maxByCreated.avtAuthor,
                    avtPartner = maxByCreated.avtPartner,
                    AuthorUsername = maxByCreated.AuthorUsername,
                    PartnerUsername = maxByCreated.PartnerUsername,
                    displayAvt = maxByCreated.whose.Equals("other") ? maxByCreated.avtAuthor : maxByCreated.avtPartner,
                    displayUsername = maxByCreated.whose.Equals("other") ? maxByCreated.AuthorUsername : maxByCreated.PartnerUsername,
                    IdToClick = maxByCreated.whose.Equals("other") ? maxByCreated.AuthorId : maxByCreated.ReceiveId,
                    type = maxByCreated.type
                };
            }).ToList();

            if (chatGroupMin.Count != 0)
            {
                foreach (var chat in chatGroupMin)
                {
                    if ((chat.whose.Equals("me") && chat.AuthorId == Int32.Parse(userId)) || (chat.whose.Equals("other") && chat.ReceiveId == Int32.Parse(userId)))
                    {
                        listFollowerDown.Add(chat);
                    }
                }
            }

            if (chatGroupOfGroup.Count != 0)
            {
                foreach (var chat in chatGroupOfGroup)
                {
                    listFollowerDown.Add(chat);
                }
            }

            listFollowerDown = listFollowerDown.OrderByDescending(x => x.createdBy).ToList();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return new JsonResult(new { data = listFollowerDown }, options);
        }
        public class MessageModel
        {
            public string messContent { get; set; }
            public int partnerId { get; set; }
        }
        public class MessageModelGroup
        {
            public string messContent { get; set; }
            public int groupId { get; set; }
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
            public string AuthorUsername { get; set; }
            public string PartnerUsername { get; set; }
            public string displayAvt { get; set; }
            public string displayUsername { get; set; }
            public int IdToClick { get; set; }
            public bool type { get; set; }
            public int GroupId { get; set; }
        }
    }
}
