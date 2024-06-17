using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN221_Assignment.Authorization;
using PRN221_Assignment.Models;
using System.Security.Claims;
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
            newThread.AuthorId = 1;
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
            //var username = User.Identity;
            //var isAuthenticate = User.Identity?.IsAuthenticated ?? false;
            //var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            //return new JsonResult(new { username, isAuthenticate, email, userId });

            Threads = context.Thread
               .Include(x => x.ThreadImages)
                .Include(x => x.ThreadComments)
                .Include(x => x.Account)
               .ThenInclude(account => account.Info)
                .OrderByDescending(x => x.SubmitDate)
              .ToList();

            foreach (var th in Threads)
            {
                int countComment = context.ThreadComment.Count(x => x.ThreadId == th.ThreadId);
                dicThreadComment[th.ThreadId.ToString()] = countComment;
            }
        }
    }
}
