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
        }
        public Dictionary<string, int> dicThreadComment { get; set; }
        public Account selectedAccount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public List<Models.Thread> myThreads { get; set; }
        public void OnGet()
        {
            selectedAccount = context.Accounts.Include(x => x.Info).FirstOrDefault(x => x.UserID == userId);

            myThreads = context.Thread.Include(x => x.ThreadImages).Where(x => x.AuthorId == userId).OrderByDescending(x => x.SubmitDate).ToList();


            foreach (var th in myThreads)
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
