using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN221_Assignment.Authorization;
using PRN221_Assignment.Models;
using System.Security.Claims;
using System.Threading;
using Thread = PRN221_Assignment.Models.Thread;

namespace PRN221_Assignment.Pages
{

    public class IndexModel : PageModel
    {
        private readonly PRN221_Assignment.Respository.DataContext context;

        public IndexModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
            dicThreadComment = new Dictionary<string, int>();
            //Thread = new Models.Thread();
        }
        [BindProperty]
        public Models.Thread Thread { get; set; }
        [BindProperty]
        public List<IFormFile> UploadedFiles { get; set; }
        public List<Thread> Threads { get; set; }
        public Dictionary<string, int> dicThreadComment { get; set; }
        public Account currentAccount { get; set; }
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
        public void OnGet(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["msg"] = msg;
            }
            if (User != null && User.Claims != null)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                currentAccount = context.Accounts.Include(x => x.Info).FirstOrDefault(x => x.UserID == Int32.Parse(userId));
                ViewData["UserId"] = userId;
            }

            Threads = context.Thread
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

        }
    }
}
