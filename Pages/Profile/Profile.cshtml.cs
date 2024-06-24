using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRN221_Assignment.Models;
using System.Security.Claims;
using System.Threading;

namespace PRN221_Assignment.Pages.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly PRN221_Assignment.Respository.DataContext context;
        public ProfileModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
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

        public void OnGet()
        {
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
