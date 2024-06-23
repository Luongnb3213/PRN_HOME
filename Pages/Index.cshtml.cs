using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PRN221_Assignment.Authorization;
using PRN221_Assignment.Models;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading;
using Thread = PRN221_Assignment.Models.Thread;
using PRN221_Assignment.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace PRN221_Assignment.Pages
{

    public class IndexModel : PageModel
    {
        private readonly PRN221_Assignment.Respository.DataContext context;
        private readonly IHubContext<chatHub> hubContext;

        public IndexModel(PRN221_Assignment.Respository.DataContext _context, IHubContext<chatHub> _hubContext)
        {
            context = _context;
            hubContext = _hubContext;
            dicThreadComment = new Dictionary<string, int>();
            dicReact = new Dictionary<string, bool>();
        }
        [BindProperty]
        public Models.Thread Thread { get; set; }
        [BindProperty]
        public List<IFormFile> UploadedFiles { get; set; }
        public List<Thread> Threads { get; set; }
        public Dictionary<string, int> dicThreadComment { get; set; }
        public Dictionary<string, bool> dicReact { get; set; }
        public Account currentAccount { get; set; }
        [BindProperty(SupportsGet = true)]
        public string typeReact { get; set; }
        [BindProperty(SupportsGet = true)]
        public int threadId { get; set; }
        public async Task<IActionResult> OnPost()
        {
            foreach (var media in UploadedFiles)
            {
                var filePath = Path.Combine("wwwroot/uploadMedia", media.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await media.CopyToAsync(stream);
                }
            }

            Thread newThread = new Thread();
            if (string.IsNullOrEmpty(Thread.Content))
            {
                newThread.Content = string.Empty;
            }
            else
            {
                newThread.Content = Thread.Content;
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            currentAccount = context.Accounts.Include(x => x.Info).FirstOrDefault(x => x.UserID == Int32.Parse(userId));
            newThread.AuthorId = currentAccount.UserID; //Cái này sau phải đổi với account khác
            newThread.React = 0;
            newThread.Share = 0;
            newThread.SubmitDate = DateTime.Now;
            context.Thread.Add(newThread);
            context.SaveChanges();

            foreach (var file in UploadedFiles)
            {
                ThreadImages newThreadImage = new ThreadImages();
                newThreadImage.ThreadId = newThread.ThreadId;
                newThreadImage.Media = $"uploadMedia/{file.FileName}";
                context.ThreadImages.Add(newThreadImage);
                context.SaveChanges();
            }
            TempData["msg"] = "CreatedThread";
            return RedirectToPage("", new { msg = TempData["msg"] });

        }
        public async Task<IActionResult> OnPostReacted()
        {
            var userId = string.Empty;
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            }
            if (typeReact.Equals("up"))
            {
                int currentReact = context.Thread.Where(x => x.ThreadId == threadId).Select(x => x.React).FirstOrDefault();
                Thread reactedThread = context.Thread.FirstOrDefault(x => x.ThreadId == threadId);
                reactedThread.React = ++currentReact;
                context.SaveChanges();

                ThreadReact newThreadReact = new ThreadReact();
                newThreadReact.UserID = Int32.Parse(userId);
                newThreadReact.threadId = threadId;
                context.ThreadReact.Add(newThreadReact);
                context.SaveChanges();

            } else
            {
                int currentReact = context.Thread.Where(x => x.ThreadId == threadId).Select(x => x.React).FirstOrDefault();
                Thread reactedThread = context.Thread.FirstOrDefault(x => x.ThreadId == threadId);
                reactedThread.React = --currentReact;
                context.SaveChanges();

                ThreadReact previousReact = context.ThreadReact.FirstOrDefault(x => x.threadId == threadId && x.UserID == Int32.Parse(userId));
                context.ThreadReact.Remove(previousReact);
                context.SaveChanges();

            }
            var currentThreadReact = context.Thread.FirstOrDefault(x => x.ThreadId == threadId);
            int currentReactAfter = currentThreadReact.React;
            await hubContext.Clients.All.SendAsync("ReceiveMessage", threadId, currentReactAfter);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return new JsonResult(new { typeReact }, options);
        }
        public void OnGet(string msg)
        {
            var userId = string.Empty;
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["msg"] = msg;
            }
            if (User != null && User.Claims != null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                currentAccount = context.Accounts.Include(x => x.Info).FirstOrDefault(x => x.UserID == Int32.Parse(userId));
                ViewData["UserId"] = userId;
            }

            Threads = context.Thread
                .Include(x => x.ThreadReacts)
               .Include(x => x.ThreadImages)
                .Include(x => x.ThreadComments)
                .Include(x => x.Account)
               .ThenInclude(account => account.Info)
                .OrderByDescending(x => x.SubmitDate)
              .ToList();

            foreach (var th in Threads)
            {
                int totalOriginal = context.ThreadComment.Count(x => x.ThreadId == th.ThreadId);
                int totalReply = context.Conversation
                            .Count(x => context.ThreadComment.Where(x => x.ThreadId == th.ThreadId).Select(x => x.ThreadCommentId)
                            .Contains(x.ThreadCommentId));

                int countComment = totalOriginal + totalReply;
                dicThreadComment[th.ThreadId.ToString()] = countComment;
            }

            foreach(var react in Threads)
            {
                if (react.ThreadReacts.Where(x => x.UserID == Int32.Parse(userId)).Count() != 0)
                {
                    dicReact[react.ThreadId.ToString()] = true;
                } else
                {
                    dicReact[react.ThreadId.ToString()] = false;
                }
            }

        }
    }
}
